using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : NPC
{
    [SerializeField]
    private float rotateDegree;

    private Quaternion nextView;

    private void Start()
    {
        currentTime = delay;
    }

    private void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        if(currentTime <= 0)
        {
            Routine();
            currentTime = delay;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

    public override void Routine()
    {
        base.Routine();
        nextView = Quaternion.Euler(transform.eulerAngles + Vector3.forward * rotateDegree);
        transform.rotation = Quaternion.Slerp(transform.rotation, nextView, 1);
    }
}
