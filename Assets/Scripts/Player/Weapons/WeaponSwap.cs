using System;
using UnityEngine;
using Mirror;

namespace Player.Weapons
{
    public class WeaponSwap : NetworkBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Weapon currentWeapon;

        private void Start()
        {
            currentWeapon = null;

            if (hasAuthority)
            {
                InGameUIManager.instance.WeaponUI.SetTargetPlayer(this);
            }
        }

        private void Update()
        {
            Debug.Log(currentWeapon);
        }

        public void SetWeapon(Weapon weapon)
        {
            if(currentWeapon != null)
            {
                parent.DetachChildren();
                currentWeapon.GetComponent<Collider2D>().enabled = true;
            }

            currentWeapon = weapon;
            weapon.transform.SetParent(parent);
            weapon.transform.localPosition = new Vector3(parent.localPosition.x, parent.localPosition.y - 3, parent.localPosition.z);
            weapon.transform.localRotation = parent.localRotation;
            InGameUIManager.instance.WeaponUI.UpdateSprite(weapon.WeaponType.ToString(), weapon.amount);
        }

        public Weapon GetWeapon()
        {
            return currentWeapon;
        }

        [Command]
        private void CmdDecreaseBullet(int number)
        {
            if (!currentWeapon) return;

            Debug.Log(netId + " sisa peluru : " + currentWeapon.amount);
            currentWeapon.amount -= number;
            UpdateAmount(currentWeapon.amount);
        }

        public void DecreaseBullet(int number)
        {
            CmdDecreaseBullet(number);
        }

        [TargetRpc]
        public void UpdateAmount(int amount)
        {
            InGameUIManager.instance.WeaponUI.UpdateAmount(amount);
        }
        //[SerializeField] private WeaponType weaponType;
        //[SerializeField] private Pistol pistol;
        //[SerializeField] private Shotgun shotgun;
        //
        //private int _tempAmount;
        //
        //private void Start()
        //{
        //    pistol.gameObject.SetActive(weaponType == WeaponType.Pistol);
        //    shotgun.gameObject.SetActive(weaponType == WeaponType.Shotgun);
        //}
        //
        //private void PistolActive()
        //{
        //    pistol.gameObject.SetActive(true);
        //    shotgun.gameObject.SetActive(false);
        //
        //    weaponType = WeaponType.Pistol;
        //}
        //
        //private void ShotgunActive()
        //{
        //    pistol.gameObject.SetActive(false);
        //    shotgun.gameObject.SetActive(true);
        //
        //    weaponType = WeaponType.Shotgun;
        //}
        //
        //private void OnTriggerStay2D(Collider2D col)
        //{
        //    if (!col.gameObject.CompareTag("Player")) return;
        //
        //    if (!Input.GetKeyDown(KeyCode.Q)) return;
        //
        //    switch (weaponType)
        //    {
        //        // if player have Pistol & weapon type item pistol
        //        case WeaponType.Pistol when PlayerManager.Instance.WeaponType == WeaponType.Pistol:
        //
        //            _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
        //            pistol.SwapWeapon(pistol.Amount);
        //            pistol.Amount = _tempAmount;
        //
        //            PistolActive();
        //            break;
        //
        //        //if player have shotgun & weapon type item shotgun
        //        case WeaponType.Pistol when PlayerManager.Instance.WeaponType == WeaponType.Shotgun:
        //
        //            _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
        //            pistol.SwapWeapon(pistol.Amount);
        //            shotgun.Amount = _tempAmount;
        //
        //            ShotgunActive();
        //            break;
        //
        //        // if player have shotgun & weapon type item pistol
        //        case WeaponType.Shotgun when PlayerManager.Instance.WeaponType == WeaponType.Pistol:
        //
        //            _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
        //            shotgun.SwapWeapon(shotgun.Amount);
        //            pistol.Amount = _tempAmount;
        //
        //            PistolActive();
        //            break;
        //
        //        // if player have shotgun & weapon type item shotgun
        //        case WeaponType.Shotgun when PlayerManager.Instance.WeaponType == WeaponType.Shotgun:
        //
        //            _tempAmount = PlayerManager.Instance.ItemPlayer.Amount;
        //            shotgun.SwapWeapon(shotgun.Amount);
        //            shotgun.Amount = _tempAmount;
        //
        //            ShotgunActive();
        //            break;
        //
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}
    }
}