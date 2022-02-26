using System.Collections;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        [SerializeField] private new Rigidbody2D rigidbody2D;

        private void Update()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(nameof(DestroyBullet));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            gameObject.SetActive(false);
        }

        public void Move(Vector2 direction)
        {
            rigidbody2D.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
    }
}