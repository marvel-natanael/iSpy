using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Player.Bullets
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;

        private float _damage = 0;

        private PlayerShoot owner = null;

        private void Update()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(nameof(DestroyBullet));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("AI")) return;

            gameObject.SetActive(false);

            // if the bullet hits another player
            if (col.gameObject.CompareTag("Player"))
            {
                Debug.Log("Fire "+col.gameObject.name);
                try
                {
                    PlayerManager target = col.GetComponent<PlayerManager>();
                    if (target == null) return;
                    target.GetComponent<PlayerManager>().DamageTo(target, _damage);
                }catch(Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        /// <summary>
        ///   <para>Moving Object Bullet</para>
        /// </summary>
        /// <param name="direction">Set direction bullet when instantiated </param>
        /// <param name="speed">Set speed bullet when instantiated</param>
        public void Move(Vector2 direction, float speed)
        {
            rigidbody2D.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        /// <summary>
        ///   <para>Set Bullet Damage When Trigger With Another Player</para>
        /// </summary>
        /// <param name="damage">Set value damage</param>
        public void Damage(float damage)
        {
            _damage = damage;
        }

        public void SetOwner(PlayerShoot player)
        {
            owner = player;
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
    }
}