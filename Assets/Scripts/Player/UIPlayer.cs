using Player.Item;
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

        private void Start()
        {
            _itemPlayer = PlayerManager.Instance.ItemPlayer;
        }

        private void Update()
        {
            textAmount.text = "Amount : " + _itemPlayer.Amount;
            textHealth.text = "Health : " + _itemPlayer.Health;
            textWeapon.text = "Weapon : " + PlayerManager.Instance.WeaponType;
        }
    }
}