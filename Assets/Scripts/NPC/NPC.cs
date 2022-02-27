using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    protected float delay;
    protected float currentTime;

    public virtual void Attack()
    {
        Debug.Log(gameObject.name + " attack!");
    }

    public virtual void Routine()
    {
        Debug.Log(gameObject.name + " do routine!");
    }
}
