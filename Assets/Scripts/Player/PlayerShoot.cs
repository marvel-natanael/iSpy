using Mirror;
using Player.Bullets;
using Player.Weapons;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;

namespace Player
{
    public class PlayerShoot : NetworkBehaviour
    {
        [Header("Properties")] [SerializeField]
        private float distance = 10f;

        // Properties
        private float _timer;

        [SyncVar][SerializeField] 
        private bool _shoot = false;

        //Component
        private Vector2 _position;
        private Weapon _selected;

        [SerializeField]private AudioSource _audioSource;
        [SerializeField] private AudioClip _clipSenjata1, _clipSenjata2;

        private WeaponSwap weapon;

        public bool GetShoot() => _shoot;
        public WeaponSwap GetWeapon() => weapon;
        
        private void Start()
        {
            weapon = GetComponent<WeaponSwap>();

            if (hasAuthority)
            {
                InGameUIManager.instance.ShootButton.SetTargetPlayer(this);
            }
        }
        
        private void Update()
        {
            if (_shoot)
            {
                SetWeapon();
            }
            
        }

        private void SetWeapon()
        { 
            if (weapon.GetWeapon())
            {
                _selected = weapon.GetWeapon();
                _position = _selected.OriginShoot.position;

                var hit = Physics2D.Raycast(_position, _selected.OriginShoot.TransformDirection(Vector2.up), distance);

                if (hit.collider)
                {
                    _timer += Time.deltaTime;
                    if ((!(_timer >= _selected.FireSpeed))) return;

                    if (_selected.amount <= 0) return;
                    
                    //sfx weapons
                    SetSfx();

                    Fire(_selected.Speed, _selected.Damage);

                    if (hasAuthority)
                    {
                        weapon.DecreaseBullet(1);
                    }

                    _timer = 0f;
                }
            }
        }

        private void SetSfx()
        {
            if (_selected.WeaponType == WeaponType.Pistol)
            {
                _audioSource.PlayOneShot(_clipSenjata1);                
            }else if (_selected.WeaponType == WeaponType.Shotgun)
            {
                _audioSource.PlayOneShot(_clipSenjata1);    
            }

        }

        private void Fire(float speed, float damage)
        {
            var bulletPool = BulletPool.Instance.GetBullet(); // get object bullet pool
            if (bulletPool == null) return; // return method if bullet pool equals null

            bulletPool.SetActive(true); // set active 
            bulletPool.transform.position = _position;

            var up = -transform.up;
            var bullet = bulletPool.GetComponent<Bullet>(); // get script bullet
            bullet.SetOwner(this);
            bullet.Move(up, speed); // call method move for moving bullet 
            bullet.Damage(damage); // set damage value
        }

        [Command]
        public void Shoot()
        {
            _shoot = !_shoot;
        }
    }
}