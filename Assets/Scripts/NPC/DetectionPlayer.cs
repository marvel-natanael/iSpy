using System;
using Mirror;
using UnityEngine;

public class DetectionPlayer : MonoBehaviour
{
    [HideInInspector]public bool detection;
    
    private void Start()
    {
        detection = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detection = false;
        }
    }
}