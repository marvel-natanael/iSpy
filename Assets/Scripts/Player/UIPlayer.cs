using Player.Item;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private Text textHealth;
        [SerializeField] private Text textAmount;
        [SerializeField] private Text textWeapon;

        private ItemPlayer _itemPlayer;

        [SerializeField] private PlayerManager playerManager;

        private void Start()
        {
            if (playerManager)
            {
                _itemPlayer = playerManager.ItemPlayer;
            }

            UpdateUI(playerManager.ItemPlayer.health, playerManager.ItemPlayer.amount);
        }

        public void SetTargetPlayer(PlayerManager player)
        {
            gameObject.SetActive(true);
            playerManager = player;
        }

        public void UpdateUI(float currentHealth, int amount)
        {
            textAmount.text = "Amount : " + amount;
            textHealth.text = "Health : " + currentHealth;
            textWeapon.text = "Weapon : " + playerManager.WeaponType;
        }
    }
}