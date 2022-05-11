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

        public void SetWeapon(Weapon weapon)
        {
            if(currentWeapon != null)
            {
                parent.DetachChildren();
                currentWeapon.GetComponent<Collider2D>().enabled = true;
                currentWeapon.ChgToDropSprite();
            }

            currentWeapon = weapon;
            weapon.transform.SetParent(parent);
            weapon.transform.localPosition = new Vector3(parent.localPosition.x, parent.localPosition.y - 1, parent.localPosition.z);
            weapon.transform.localRotation = parent.localRotation;
            weapon.ChgToMountSprite();
            if (hasAuthority)
            {
                InGameUIManager.instance.WeaponUI.UpdateSprite(weapon.WeaponType.ToString(), weapon.amount);
            }
        }

        public Weapon GetWeapon()
        {
            return currentWeapon;
        }

        [Command]
        private void CmdDecreaseBullet(int number)
        {
            if (!currentWeapon) return;

            if (currentWeapon.amount <= 0) return;

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
    }
}