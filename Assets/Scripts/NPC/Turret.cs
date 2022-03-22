using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : NPC
{
    [Header("Turret Properties")]
    [SerializeField]
    private float rotateDegree;

    private Quaternion nextView;

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
        if(timerToDelay <= 0)
        {
            Routine();
            timerToDelay = delay;
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
}
