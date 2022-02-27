using System;
using UnityEngine;

namespace Player
{
    public class ItemPickUp : MonoBehaviour
    {
        [SerializeField] private ItemChoice itemChoice;

        private void OnTriggerEnter2D(Collider2D col)
        {

            if (!col.gameObject.CompareTag("Player")) return;
            
            switch (itemChoice)
            {
                case ItemChoice.Amount:
                    PickUpItemAmount();
                    break;
                case ItemChoice.Health:
                    PickUpItemHealth();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Destroy(gameObject);
        }

        private void PickUpItemHealth()
        {
            if (PlayerManager.Instance.ItemPlayer.Health >= 90)
            {
                PlayerManager.Instance.ItemPlayer.Health = 100;
            }
            else
                PlayerManager.Instance.ItemPlayer.Health += 10;
        }

        private void PickUpItemAmount()
        {
            PlayerManager.Instance.ItemPlayer.Amount += 10;
        }
    }
}