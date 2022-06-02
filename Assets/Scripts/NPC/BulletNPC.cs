using System;
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
            try
            {
                PlayerManager target = col.GetComponent<PlayerManager>();
                if (target == null) return;
                target.GetComponent<PlayerManager>().DamageTo(target, _damage);
                target.GetComponent<PlayerManager>().UpdateSprite(col.GetComponent<SpriteRenderer>(), Color.red);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        _damage = damage;
    }
}