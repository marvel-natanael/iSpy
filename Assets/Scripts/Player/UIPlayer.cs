using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private Text textHealth;
        [SerializeField] private Text textAmount;

        private ItemPlayer _itemPlayer;

        private void Start()
        {
            _itemPlayer = PlayerManager.Instance.ItemPlayer;
        }

        private void Update()
        {
            textAmount.text = "Amount : " + _itemPlayer.Amount;
            textHealth.text = "Health : " + _itemPlayer.Health;
        }
    }
}