using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Soldier : NPC
{
    [Header("Soldier Properties")]
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Transform nextMove;
    private Vector2 currentPos;

    public override void Start()
    {
        base.Start();
        currentPos = transform.position;
        timerToDelay = delay;
    }

    private void Update()
    {
        Routine();
        Attack();
    }

    public override void Routine()
    {
        base.Routine();
        if (!nextMove)
        {
            Debug.LogError("NextMove field is empty!");
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, nextMove.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextMove.position) <= 0)
        {
            if (timerToDelay <= 0)
            {
                nextMove.position = currentPos;
                currentPos = transform.position;
                Vector2 dir = nextMove.position - transform.position;

                timerToDelay = delay;
            }
            else
            {
                timerToDelay -= Time.deltaTime;
            }
        }
    }

    public override void Attack()
    {
        if (!fov.ClosestPlayer)
        {
            targetPlayer = null;
            return;
        }
        
        targetPlayer = fov.ClosestPlayer;

        base.Attack();
    }
}
