using UnityEngine;

namespace Player.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private WeaponType weaponType;
        [SerializeField] protected Transform originShoot;

        [Header("Properties")]
        [SerializeField] protected float damage;
        [SerializeField] protected float speed;
        public int amount;
        [SerializeField] protected float fireSpeed;

        protected PlayerManager playerManager;

        private void Start()
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }

        public virtual float Damage => damage;
        public virtual float Speed => speed;

        public virtual float FireSpeed => fireSpeed;

        public virtual Transform OriginShoot => originShoot;

        public virtual int Amount
        {
            get => amount;
            set => amount = value;
        }

        public virtual void SwapWeapon(int amount)
        {
            playerManager.WeaponType = weaponType;
        }
        
        public WeaponType WeaponType => weaponType;
    }
}