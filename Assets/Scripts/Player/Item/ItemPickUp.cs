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

            PlayerManager player = col.GetComponent<PlayerManager>();
        
            switch (itemChoice)
            {
                case ItemChoice.Amount:
                    PickUpItemAmount(bulletType, player);
                    break;
                case ItemChoice.Health:
                    PickUpItemHealth(player);
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
        
        private void PickUpItemAmount(WeaponType type, PlayerManager player)
        {
            player.SetWeapon(type);
            Destroy(gameObject);
        }
    }
}