using System;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Transform origin;

        [Header("Properties")] [SerializeField]
        private float distance = 10f;

        [SerializeField] private float fireSpeed = 1f;

        private float _timer;

        private Vector2 _position;
        private ItemPlayer _itemPlayer;

        private void Start()
        {
            _itemPlayer = new ItemPlayer
            {
                Amount = 5
            };
        }

        private void Update()
        {
            _position = origin.position;
            
            Debug.DrawRay(_position, origin.TransformDirection(Vector2.up) * distance, Color.black);
            var hit = Physics2D.Raycast(_position, origin.TransformDirection(Vector2.up), distance);

            if (hit)
            {
                Debug.Log(hit.collider.name);
                Fire(Time.deltaTime);
            }
        }

        private void Fire(float deltaTime)
        {
            _timer += deltaTime;

            if ((!(_timer >= fireSpeed))) return;
            
            if (_itemPlayer.Amount <= 0) return;

            var bulletPool = BulletPool.Instance.GetBullet();
            if (bulletPool == null) return;

            bulletPool.SetActive(true);
            bulletPool.transform.position = _position;

            var up = transform.up;
            up = Vector3.MoveTowards(up, _position, distance * Time.deltaTime);

            var bullet = bulletPool.GetComponent<Bullet>();
            bullet.Move(up);

            _itemPlayer.Amount -= 1;

            transform.up = up;

            _timer = 0f;
        }
    }
}