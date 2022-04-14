using UnityEngine;

public class Turret : NPC
{
    [Header("Turret Properties")] [SerializeField]
    private float rotateDegree;

    private Quaternion nextView;

    private int _indexState = 0;
    private TurretAnimation.State _state;

    public Turret()
    {
        _state = (TurretAnimation.State) _indexState;
    }

    public override void Start()
    {
        base.Start();
        timerToDelay = delay;
    }

    private void Update()
    {
        Rotate();
        Attack();
    }

    public void Rotate()
    {
        if (timerToDelay <= 0)
        {
            // Routine();
            _state = (TurretAnimation.State) _indexState;
            timerToDelay = delay;
            _indexState++;

            if (_indexState == 4) _indexState = 0;
        }
        else
        {
            timerToDelay -= Time.deltaTime;
        }
    }

    public override void Routine()
    {
        base.Routine();
        nextView = Quaternion.Euler(transform.eulerAngles + Vector3.forward * rotateDegree);
        transform.rotation = Quaternion.Slerp(transform.rotation, nextView, 1);
    }

    public override void Attack()
    {
        if (!fov.LowestHPPlayer)
        {
            targetPlayer = null;
            return;
        }

        targetPlayer = fov.LowestHPPlayer;

        base.Attack();
    }

    public TurretAnimation.State GetState => _state;
}