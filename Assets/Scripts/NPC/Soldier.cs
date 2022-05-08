using Mirror;
using UnityEngine;

public class Soldier : NetworkBehaviour
{
    [Header("Target")] [SerializeField] protected GameObject targetPlayer, child;

    [SerializeField] protected GameObject bullet;

    [Header("Properties")] [SerializeField]
    protected float delay;

    protected float timerToDelay;

    [Header("Soldier Properties")] [SerializeField]
    private float moveSpeed;

    public Vector2 maxDistance;

    [SerializeField] protected float fireSpeed;
    protected float timerToFire;

    [Header("Shoot Properties")] [SerializeField]
    protected float bulletSpeed;

    [SerializeField] protected float damage;

    [SerializeField] protected Transform _originShoot;

    [SerializeField] private DetectionPlayer _detectionPlayer;

    private Animator _animator;

    [SyncVar]
    public bool moveForward;

    public void Start()
    {
        _animator = GetComponent<Animator>();

        maxDistance = transform.position + (transform.up * 7);

        Flip(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        timerToDelay = delay;
    }

    private void Update()
    {
        if (_detectionPlayer.detection)
        {
            Attack();
        }

        NpcMove();
    }

    private void NpcMove()
    {
        if (isServer)
        {
            if (_detectionPlayer.detection)
            {
                Move(0);
            }
            else
            {
                Move(moveSpeed);
            }
        }
    }

    [Server]
    private void Move(float speed)
    {
        if (moveForward)
        {
            transform.position += transform.up * speed * Time.deltaTime;

            if (transform.position.magnitude > maxDistance.magnitude)
            {
                moveForward = false;
                Flip(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            transform.position += -transform.up * speed * Time.deltaTime;

            if (transform.position.magnitude > maxDistance.magnitude)
            {
                moveForward = true;
                Flip(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void Flip(float x, float y, float z)
    {
        transform.localScale = new Vector3(x, y, z);
        child.transform.localEulerAngles = new Vector3(0, 0, child.transform.localEulerAngles.z + 180);
    }

    private void Attack()
    {
        timerToFire += Time.deltaTime;
        if (timerToFire < fireSpeed) return;

        var objBullet = Instantiate(bullet, _originShoot.position, _originShoot.rotation);
        objBullet.GetComponent<Rigidbody2D>().velocity =
            (transform.localScale.y < 0) ? _originShoot.up * -bulletSpeed : _originShoot.up * bulletSpeed;
        objBullet.GetComponent<BulletNPC>().Damage(damage);

        Destroy(objBullet, 5);

        timerToFire = 0;
    }
}