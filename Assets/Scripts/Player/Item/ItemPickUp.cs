using System;
using Player.Weapons;
using UnityEngine;

namespace Player.Item
{
    public class ItemPickUp : MonoBehaviour
    {
        public ItemChoice itemChoice;

        public WeaponType bulletType;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;

            var player = col.gameObject;
        
            switch (itemChoice)
            {
                case ItemChoice.Amount:
                    PickUpItemAmount(player.GetComponent<WeaponSwap>());
                    break;
                case ItemChoice.Health:
                    PickUpItemHealth(player.GetComponent<PlayerManager>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PickUpItemHealth(PlayerManager player)
        {
            player.Heal(10);
            Destroy(gameObject);
        }
        
        private void PickUpItemAmount(WeaponSwap player)
        {
            player.SetWeapon(gameObject.GetComponent<Weapon>());
        }
    }
}