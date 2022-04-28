using Mirror;
using Player;
using UnityEngine;

public class BulletNPC : NetworkBehaviour
{
    private float _damage = 0;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var target = col.gameObject.GetComponent<PlayerManager>();

            target.GetComponent<PlayerManager>().DamageTo(target, _damage);
            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        _damage = damage;
    }
}