using System;
using Player.Weapons;
using UnityEngine;

namespace Player.Item
{
    public class ItemPickUp : MonoBehaviour
    {
        public ItemChoice itemChoice;

        public WeaponType bulletType;

        private void Start()
        {
            Debug.Log(bulletType);
            Debug.Log(PlayerManager.Instance.WeaponType);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {

            if (!col.gameObject.CompareTag("Player")) return;

            switch (itemChoice)
            {
                case ItemChoice.Amount:
                    PickUpItemAmount(bulletType);
                    break;
                case ItemChoice.Health:
                    PickUpItemHealth();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PickUpItemHealth()
        {
            if (PlayerManager.Instance.ItemPlayer.Health >= 90)
            {
                PlayerManager.Instance.ItemPlayer.Health = 100;
            }
            else
                PlayerManager.Instance.ItemPlayer.Health += 10;
            
            Destroy(gameObject);
        }

        private void PickUpItemAmount(WeaponType type)
        {
            if (type == PlayerManager.Instance.WeaponType)
            {
                PlayerManager.Instance.ItemPlayer.Amount += 1000;
                Destroy(gameObject);
            }
        }
    }
}