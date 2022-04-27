using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

public class BulletNPC : NetworkBehaviour
{
    private NetworkIdentity _identity = null;
    private float _damage = 0;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if the bullet hits another player
        if (col.gameObject.CompareTag("Player"))
        {
            var target = col.gameObject.GetComponent<PlayerManager>();
            if (target.netIdentity.netId != _identity.netId)
            {
                target.GetComponent<PlayerManager>().DamageTo(target, _damage);
                
                Destroy(gameObject);
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    public void SetIdentity(NetworkIdentity identity)
    {
        _identity = identity;
    }

    public void Damage(float damage)
    {
        _damage = damage;
    }
}