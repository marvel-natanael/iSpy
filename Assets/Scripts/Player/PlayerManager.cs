using System;
using Player.Item;
using Player.Weapons;
using UnityEngine;
using Mirror;
using System.Collections;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        //[SerializeField] private Weapon pistol, shotgun;

        public ItemPlayer ItemPlayer { get; set; }
        public WeaponType WeaponType { get; set; }

        private WeaponSwap weapon;

        public string playerName;

        [SerializeField] private AudioSource _source;

        [SerializeField] private PlayerAnimation _animation;

        public delegate void OnDeath();

        public static event OnDeath onDeath;


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
            AddName(PlayerNameInput.DisplayName);
            InGameUIManager.instance.PlayerUI.SetTargetPlayer(this);
        }

        [TargetRpc]
        private void RPCCheckHealth(NetworkConnection conn)
        {
            _animation.Death();
            _source.PlayOneShot(_source.clip);
        }

        private void Update()
        {
            //GetHealth(connectionToClient);
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
            if (ItemPlayer.health <= 0)
            {
                Debug.Log("Die CMD");
                CmdDead(this);
            }

            RpcUpdateUI(ItemPlayer.health);
        }

        [Command]
        private void CmdAddPlayerToServer()
        {
            GameManager.instance.playersCount += 1;
            GameManager.instance.players.Add(gameObject);
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
            RpcUpdateUIOtherPlayer(p.connectionToClient, p.ItemPlayer.health);
        }

        #endregion Attack

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
            RpcUpdateUI(ItemPlayer.health);
        }

        #endregion Heal

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

            RpcUpdateUI(ItemPlayer.health);
        }

        #endregion Bullet

        #region Lose

        public void CmdDead(PlayerManager p)
        {
             _source.PlayOneShot(_source.clip);
            GameManager.instance.playersCount -= 1;

            onDeath?.Invoke();

            RPCCheckHealth(p.connectionToClient);
            RpcShowLoseText(p.connectionToClient);
            //GameManager.instance.GameOver();
            StartCoroutine("Death", p);
        }

        IEnumerator Death(PlayerManager p)
        {
            yield return new WaitForSeconds(1);
            Destroy(p.gameObject);
        }

        [TargetRpc]
        private void RpcShowLoseText(NetworkConnection conn)
        {
            InGameUIManager.instance.LoseText.gameObject.SetActive(true);
        }

        #endregion Lose

        #region RPC UI

        [TargetRpc]
        private void RpcUpdateUIOtherPlayer(NetworkConnection conn, float currHealth)
        {
            InGameUIManager.instance.PlayerUI.UpdateUI(currHealth);
        }

        [TargetRpc]
        private void RpcUpdateUI(float currHealth)
        {
            InGameUIManager.instance.PlayerUI.UpdateUI(currHealth);
        }

        #endregion RPC UI

        public void UpdateSprite(SpriteRenderer spriteRenderer, Color color)
        {
            StartCoroutine(UpdateSpriteColor(spriteRenderer, color));
        }

        private IEnumerator UpdateSpriteColor(SpriteRenderer spriteRenderer, Color color)
        {
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
        }
    }
}