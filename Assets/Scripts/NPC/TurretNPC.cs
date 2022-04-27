using System;
using Mirror;
using UnityEngine;

public class TurretNPC : NetworkBehaviour
{

    [SerializeField] private GameObject bullet;

    private float _timerToFire;

    [Header("Shoot Properties")]
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float damage;
    [SerializeField] private float fireSpeed;
    
    [Header("Turret Properties")]
    [SyncVar] public float health = 100;

    [Header("Turret Components")]
    [SerializeField] private Transform originShoot;
    [SerializeField] private Transform turretWeapon;

    private void Update()
    {
        Rotate();
        Attack();
    }

    private void Rotate()
    {
        turretWeapon.Rotate(0, 0, 0.1f, Space.Self);
    }

    private void Attack()
    {
        _timerToFire += Time.deltaTime;
        if (_timerToFire < fireSpeed) return;

        var objBullet = Instantiate(bullet, originShoot.position, Quaternion.identity);
        
        objBullet.GetComponent<Rigidbody2D>().velocity = turretWeapon.up * bulletSpeed;
        objBullet.GetComponent<BulletNPC>().Damage(damage);
        objBullet.GetComponent<BulletNPC>().SetIdentity(netIdentity);
        
        NetworkServer.Spawn(objBullet);
        
        Destroy(objBullet, 5);

        _timerToFire = 0;
    }
}