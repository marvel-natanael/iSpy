using System;
using UnityEngine;

namespace Player.Weapons
{
    public class WeaponSwap : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private Pistol pistol;
        [SerializeField] private Shotgun shotgun;

        private int _tempAmount;

        private void Start()
        {
            pistol.gameObject.SetActive(weaponType == WeaponType.Pistol);
            shotgun.gameObject.SetActive(weaponType == WeaponType.Shotgun);
        }

        private void PistolActive()
        {
            pistol.gameObject.SetActive(true);
            shotgun.gameObject.SetActive(false);

            weaponType = WeaponType.Pistol;
        }

        private void ShotgunActive()
        {
            pistol.gameObject.SetActive(false);
            shotgun.gameObject.SetActive(true);

            weaponType = WeaponType.Shotgun;
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;

            if (!Input.GetKey(KeyCode.Q)) return;

            switch (weaponType)
            {
                // if player have Pistol & weapon type item pistol
                case WeaponType.Pistol when PlayerManager.Instance.WeaponType == WeaponType.Pistol:

                    _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
                    pistol.SwapWeapon(pistol.Amount);
                    pistol.Amount = _tempAmount;

                    PistolActive();
                    break;

                //if player have shotgun & weapon type item shotgun
                case WeaponType.Pistol when PlayerManager.Instance.WeaponType == WeaponType.Shotgun:

                    _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
                    pistol.SwapWeapon(pistol.Amount);
                    shotgun.Amount = _tempAmount;

                    ShotgunActive();
                    break;

                // if player have shotgun & weapon type item pistol
                case WeaponType.Shotgun when PlayerManager.Instance.WeaponType == WeaponType.Pistol:

                    _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
                    shotgun.SwapWeapon(shotgun.Amount);
                    pistol.Amount = _tempAmount;

                    PistolActive();
                    break;

                // if player have shotgun & weapon type item shotgun
                case WeaponType.Shotgun when PlayerManager.Instance.WeaponType == WeaponType.Shotgun:

                    _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
                    shotgun.SwapWeapon(shotgun.Amount);
                    shotgun.Amount = _tempAmount;

                    ShotgunActive();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}