using Player.Bullets;
using Player.Item;
using Player.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        private float distance = 10f;

        // Properties
        private float _timer;
        private bool _shoot;

        //Component
        private Vector2 _position;
        private ItemPlayer _itemPlayer;
        private Weapon _selected;

        private void Start()
        {
            _selected = PlayerManager.Instance.GetWeapon();
            _itemPlayer = PlayerManager.Instance.ItemPlayer;
            _itemPlayer.Amount = _selected.Amount; // update ui amount bullet weapon
        }

        private void Update()
        {
            _position = _selected.OriginShoot.position;

            // updated data from player manager
            _itemPlayer = PlayerManager.Instance.ItemPlayer; // for updated item player
            _selected = PlayerManager.Instance.GetWeapon(); // for updated weapon player

            Debug.DrawRay(_position, _selected.OriginShoot.TransformDirection(Vector2.up) * distance, Color.black);
            var hit = Physics2D.Raycast(_position, _selected.OriginShoot.TransformDirection(Vector2.up), distance);

            if (hit && _shoot)
            {
                Debug.Log(hit.collider.name);
                SetWeapon();
            }
        }

        private void SetWeapon()
        {
            _timer += Time.deltaTime;
            if ((!(_timer >= _selected.FireSpeed))) return;

            if (_itemPlayer.Amount <= 0) return; // if amount bullet <= 0

            Fire(_selected.Speed, _selected.Damage); // method for fire weapon

            _itemPlayer.Amount -= 1; // decrement amount bullet weapon

            _timer = 0f;
            
            // set timer to 0
        }

        private void Fire(float speed, float damage)
        {
            var bulletPool = BulletPool.Instance.GetBullet(); // get object bullet pool
            if (bulletPool == null) return; // return method if bullet pool equals null

            bulletPool.SetActive(true); // set active 
            bulletPool.transform.position = _position;

            var up = transform.up;
            up = Vector3.MoveTowards(up, _position, distance * Time.deltaTime);

            var bullet = bulletPool.GetComponent<Bullet>(); // get script bullet
            bullet.Move(up, speed); // call method move for moving bullet 
            bullet.Damage(damage); // set damage value

            transform.up = up;
        }

        public void Shoot()
        {
            _shoot = !_shoot;
        }
    }
}