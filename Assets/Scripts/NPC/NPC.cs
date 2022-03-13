using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class NPC : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected GameObject targetPlayer;

    [Header("Properties")]
    [SerializeField]
    protected float delay;
    protected float timerToDelay;

    protected FieldOfView fov;

    [SerializeField] private float fireSpeed;
    private float timerToFire;

    public virtual void Start()
    {
        fov = GetComponent<FieldOfView>();
        timerToDelay = delay;
    }

    public virtual void Attack()
    {
        timerToFire += Time.deltaTime;
        if (timerToFire < fireSpeed) return;

        if (fov.playersInRange.Count == 0) return;

        var bulletPool = BulletPool.Instance.GetBullet();
        if (bulletPool == null) return;

        bulletPool.SetActive(true);
        bulletPool.transform.position = transform.position;

        var dir = targetPlayer.transform.position - transform.position;
        var bullet = bulletPool.GetComponent<Bullet>();
        bullet.Move(dir);

        timerToFire = 0;
        Debug.Log(gameObject.name + " attack!");
    }

    public virtual void Routine()
    {
        //Debug.Log(gameObject.name + " do routine!");
    }
}
public enum NPCType
{
    Soldier,
    Turret
}
