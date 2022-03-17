using System;
using Player.Item;
using Player.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Weapon pistol, shotgun;

        public static PlayerManager Instance;

        public ItemPlayer ItemPlayer { get; set; }
        public WeaponType WeaponType { get; set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;

            ItemPlayer = new ItemPlayer
            {
                Health = 100
            };

            WeaponType = WeaponType.Pistol;
            SetWeapon(WeaponType);
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
            if (ItemPlayer.Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}