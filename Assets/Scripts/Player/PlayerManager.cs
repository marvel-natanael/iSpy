using System;
using Player.Item;
using Player.Weapons;
using UnityEngine;
using Mirror;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private Weapon pistol, shotgun;

        //public static PlayerManager Instance;

        public ItemPlayer ItemPlayer { get; set; }
        public WeaponType WeaponType { get; set; }

        private void Awake()
        {
            //if (Instance == null) Instance = this;

            ItemPlayer = new ItemPlayer
            {
                health = 100
            };

            WeaponType = WeaponType.Pistol;
            SetWeapon(WeaponType);
        }

        private void Start()
        {
            if (!hasAuthority) return;

            InGameUIManager.instance.PlayerUI.SetTargetPlayer(this);

        }

        public Weapon GetWeapon()
        {
            return WeaponType switch
            {
                WeaponType.Pistol => pistol,
                WeaponType.Shotgun => shotgun,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SetWeapon(WeaponType type)
        {
            pistol.gameObject.SetActive(type == WeaponType.Pistol);
            shotgun.gameObject.SetActive(type == WeaponType.Shotgun);
        }

        private void Update()
        {
            
        }

        public void DamageTo(PlayerManager p, float dmg)
        {
            CmdDamageTo(p, dmg);
        }

        [Command]
        private void CmdDamageTo(PlayerManager p, float dmg)
        {
            if(p.ItemPlayer.health <= 0)
            {
                CmdDead(p);
                return;
            }

            p.ItemPlayer.health -= dmg;
            RpcUpdateUI(p.connectionToClient, p.ItemPlayer.health);
        }

        [TargetRpc]
        private void RpcUpdateUI(NetworkConnection conn, float currHealth)
        {
            InGameUIManager.instance.PlayerUI.UpdateUI(currHealth);
        }

        [Command]
        private void CmdDead(PlayerManager p)
        {
            RpcShowLoseText(p.connectionToClient);
            Destroy(p.gameObject);
        }

        [TargetRpc]
        private void RpcShowLoseText(NetworkConnection conn)
        {
            InGameUIManager.instance.LoseText.gameObject.SetActive(true);
        }
    }
}