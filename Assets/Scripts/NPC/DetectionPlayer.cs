using System;
using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class DetectionPlayer : MonoBehaviour
{
    [HideInInspector]public bool detection;

    private List<GameObject> playerInDetection = new List<GameObject>();

    private void Start()
    {
        detection = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detection = true;

            if (!playerInDetection.Contains(other.gameObject))
            {
                playerInDetection.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detection = false;

            if (playerInDetection.Contains(other.gameObject))
            {
                playerInDetection.Remove(other.gameObject);
            }
        }
    }

    public List<GameObject> GetListPlayer()
    {
        return playerInDetection;
    }
}