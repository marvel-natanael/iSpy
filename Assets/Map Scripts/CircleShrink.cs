using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShrink : MonoBehaviour
{
    Collider2D shrinkCollider;
    bool isOutside = true;
    void Start()
    {
        shrinkCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if(isOutside)
        {
            Damage();
        }
    }

    void Damage()
    {
        Debug.Log("is damaged");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOutside = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
