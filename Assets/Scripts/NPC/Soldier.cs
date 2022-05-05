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

    [SerializeField] private Transform nextMove;
    private Vector2 currentPos;

    [SerializeField] protected float fireSpeed;
    protected float timerToFire;

    [Header("Shoot Properties")] [SerializeField]
    protected float bulletSpeed;

    [SerializeField] protected float damage;

    [SerializeField] protected Transform _originShoot;

    [SerializeField] private DetectionPlayer _detectionPlayer;

    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        
        currentPos = transform.position;
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y,
            transform.localScale.z);
        timerToDelay = delay;
    }

    private void Update()
    {
        if (_detectionPlayer.detection)
        {
            Attack();
        }

        if (_detectionPlayer.detection)
        {
            //Debug.Log("Detection True " + _detectionPlayer.detection);
            Move(transform.position, 0);
        }
        else
        {
            //Debug.Log("Detection False " + _detectionPlayer.detection);
            Move(nextMove.position, 2f);
        }

        Routine();
    }

    private void Routine()
    {
        if (!nextMove)
        {
            //Debug.LogError("NextMove field is empty!");
            return;
        }

        if (Vector2.Distance(transform.position, nextMove.position) <= 0)
        {
            if (timerToDelay <= 0)
            {
                if (!_animator.enabled) _animator.enabled = true;

                nextMove.position = currentPos;
                currentPos = transform.position;

                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y,
                    transform.localScale.z);

                child.transform.localEulerAngles = new Vector3(0, 0, child.transform.localEulerAngles.z + 180);
                timerToDelay = delay;
            }
            else
            {
                _animator.enabled = false;
                timerToDelay -= Time.deltaTime;
            }
        }
    }

    private void Move(Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
        //Debug.Log(gameObject.name + " attack!");
    }
}