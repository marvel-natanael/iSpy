using Player.Item;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private Text textHealth;

        private ItemPlayer _itemPlayer;

        [SerializeField] private PlayerManager playerManager;

        private void Start()
        {
            if (playerManager)
            {
                _itemPlayer = playerManager.ItemPlayer;
            }

            UpdateUI(playerManager.ItemPlayer.health);
        }

        public void SetTargetPlayer(PlayerManager player)
        {
            gameObject.SetActive(true);
            playerManager = player;
        }

        public void UpdateUI(float currentHealth)
        {
            textHealth.text = "Health : " + currentHealth;
        }
    }
}