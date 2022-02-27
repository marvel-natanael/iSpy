using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArea : MonoBehaviour
{
    private List<GameObject> playersInRange = new List<GameObject>();
    public List<GameObject> PlayerInRange { get { return playersInRange; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playersInRange.Contains(collision.gameObject))
        {
            playersInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playersInRange.Contains(collision.gameObject))
        {
            playersInRange.Remove(collision.gameObject);
        }
    }
}
