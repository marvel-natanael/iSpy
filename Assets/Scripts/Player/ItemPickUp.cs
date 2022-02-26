using System;
using UnityEngine;

namespace Player
{
    public class ItemPickUp : MonoBehaviour
    {
        [SerializeField] private ItemChoice itemChoice;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                switch (itemChoice)
                {
                    case ItemChoice.Amount:
                        
                        break;
                    case ItemChoice.Health :
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}