using Player.Item;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private Image bar;

        [SerializeField] private PlayerManager playerManager;

        private void Start()
        {
            UpdateUI(playerManager.ItemPlayer.health);
        }

        public void SetTargetPlayer(PlayerManager player)
        {
            gameObject.SetActive(true);
            playerManager = player;
        }

        public void UpdateUI(float currentHealth)
        {
            bar.fillAmount = currentHealth / 100;
        }
    }
}