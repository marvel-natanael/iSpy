using Mirror;
using Player.Bullets;
using UnityEngine;

public class NPC : NetworkBehaviour
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

    [Header("Shoot Properties")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage;

    [SyncVar]
    public float health = 100;

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
        bullet.Move(dir, bulletSpeed);
        bullet.Damage(damage);

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
