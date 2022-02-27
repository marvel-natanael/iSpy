using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NPC
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform nextMove;
    private Vector2 currentPos;

    private float closestPlayerDist;
    private GameObject targetPlayer;

    private void Start()
    {
        currentPos = transform.position;
        currentTime = delay;
    }

    private void Update()
    {
        Routine();
    }

    public override void Routine()
    {
        base.Routine();
        if (!nextMove)
        {
            Debug.LogError("NextMove field is empty!");
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, nextMove.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextMove.position) <= 0)
        {
            if (currentTime <= 0)
            {
                nextMove.position = currentPos;
                currentPos = transform.position;

                currentTime = delay;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }

    public override void Attack()
    {
        base.Attack();
        var shootArea = GetComponentInChildren<ShootArea>();
        var playersInArea = shootArea.PlayerInRange;
        //closestPlayerDist = max radius area

        foreach(var player in playersInArea)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);

            if(dist < closestPlayerDist)
            {
                closestPlayerDist = dist;
                targetPlayer = player;
            }
        }
    }
}
