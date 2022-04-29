using System;
using Player.Item;
using Player.Weapons;
using UnityEngine;
using Mirror;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        //[SerializeField] private Weapon pistol, shotgun;

        public ItemPlayer ItemPlayer { get; set; }
        public WeaponType WeaponType { get; set; }

        private WeaponSwap weapon;

        public static event Action OnGameOver;

        public string playerName;

        private void Awake()
        {
            //WeaponType = WeaponType.Pistol;
            //SetWeapon(WeaponType);
            //weapon = gameObject.GetComponent<WeaponSwap>();

            ItemPlayer = new ItemPlayer
            {
                health = 100
            };

            //ItemPlayer.amount = weapon.GetWeapon().amount;

        }

        private void Start()
        {
            if (!hasAuthority) return;

            CmdAddPlayerToServer();
            AddName(PlayerNameInput.displayName);
            InGameUIManager.instance.PlayerUI.SetTargetPlayer(this);
        }

        private void Update()
        {
            //Debug.Log("Health Player : "+ItemPlayer.health);
        }

        //public Weapon GetWeapon()
        //{
        //    return WeaponType switch
        //    {
        //        WeaponType.Pistol => pistol,
        //        WeaponType.Shotgun => shotgun,
        //        _ => throw new ArgumentOutOfRangeException()
        //    };
        //}

        //public void SetWeapon(Weapon weapon)
        //{
        //    //pistol.gameObject.SetActive(type == WeaponType.Pistol);
        //    //shotgun.gameObject.SetActive(type == WeaponType.Shotgun);
        //}

        public void AddName(string name)
        {
            if (isServer) return;
            CmdAddName(name);
        }


        [Command]
        private void CmdAddName(string name)
        {
            playerName = name;
            GameManager.instance.playerNames.Add(playerName);
        }


        public void TakeDamage(float damage)
        {
            if (isServer) return;
            CmdTakeDamage(damage);
        }

        [Command]
        private void CmdTakeDamage(float damage)
        {
            ItemPlayer.health -= damage;
            if(ItemPlayer.health <= 0)
            {
                CmdDead(this);
            }
            RpcUpdateUI(ItemPlayer.health, ItemPlayer.amount);
        }

        [Command]
        private void CmdAddPlayerToServer()
        {
            GameManager.instance.playersCount += 1;
        }

        #region Attack
        public void DamageTo(PlayerManager p, float dmg)
        {
            if (isServer) return;
            if (!hasAuthority) return;
            
            CmdDamageTo(p, dmg);
        }

        [Command]
        private void CmdDamageTo(PlayerManager p, float dmg)
        {
            if (p.ItemPlayer.health <= 0)
            {
                CmdDead(p);
                return;
            }

            p.ItemPlayer.health -= dmg;
            RpcUpdateUIOtherPlayer(p.connectionToClient, p.ItemPlayer.health, ItemPlayer.amount);
        }
        #endregion

        #region Heal
        public void Heal(float health)
        {
            if (isServer) return;

            CmdHeal(health);
        }

        [Command]
        private void CmdHeal(float health)
        {
            if (ItemPlayer.health >= 100) return;
            ItemPlayer.health += health;
            RpcUpdateUI(ItemPlayer.health, ItemPlayer.amount);
        }
        #endregion

        #region Bullet
        public void DecreaseAmountBullet()
        {
            Debug.Log(netId + " client amount : " + ItemPlayer.amount);
            CmdDecreaseAmountBullet();
        }

        [Command]
        private void CmdDecreaseAmountBullet()
        {
            Debug.Log(netId + " amount : " + ItemPlayer.amount);
            ItemPlayer.amount -= 1;

            RpcUpdateUI(ItemPlayer.health, ItemPlayer.amount);
        }
        #endregion

        #region Lose
        public void CmdDead(PlayerManager p)
        {
            GameManager.instance.playersCount -= 1;
            RpcShowLoseText(p.connectionToClient);
            OnGameOver();
            //GameManager.instance.GameOver();
            Destroy(p.gameObject);
        }

        [TargetRpc]
        private void RpcShowLoseText(NetworkConnection conn)
        {
            InGameUIManager.instance.LoseText.gameObject.SetActive(true);
        }
        #endregion

        #region RPC UI
        [TargetRpc]
        private void RpcUpdateUIOtherPlayer(NetworkConnection conn, float currHealth, int amount)
        {
            InGameUIManager.instance.PlayerUI.UpdateUI(currHealth, amount);
        }

        [TargetRpc]
        private void RpcUpdateUI(float currHealth, int amount)
        {
            InGameUIManager.instance.PlayerUI.UpdateUI(currHealth, amount);
        }
        #endregion
        public override void OnStopClient()
        {
            //RemoveName(playerName);
        }
    }
}