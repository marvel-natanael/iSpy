using System.Collections.Generic;
using Player;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Properties")] public NPCType npcType;
    [SerializeField] private float range;
    [Range(0, 360)] public float angle;

    [Header("Target Data")] public List<GameObject> playersInRange;
    private GameObject closestPlayer = null;
    private GameObject lowestHPPlayer = null;

    private float closestDistance;

    private CircleCollider2D collider;

    public GameObject ClosestPlayer
    {
        get { return closestPlayer; }
    }

    public GameObject LowestHPPlayer
    {
        get { return lowestHPPlayer; }
    }

    private void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        collider.radius = range;
    }

    private void DetectPlayerInFieldOfView(GameObject player)
    {
        Vector2 dir = player.transform.position - transform.position;
        float playerAngle = Vector2.Angle(dir, transform.up);

        if (playerAngle < angle / 2)
        {
            if (!playersInRange.Contains(player))
            {
                playersInRange.Add(player);
            }
        }
        else
        {
            if (playersInRange.Contains(player))
            {
                playersInRange.Remove(player);
            }
        }
    }

    private void FindClosestPlayer()
    {
        closestDistance = range;

        foreach (GameObject g in playersInRange)
        {
            float currentDistance = Vector2.Distance(g.transform.position, transform.position);

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestPlayer = g;
            }
        }
    }

    private void FindLowestHP()
    {
        float lowestHP = 100;

        foreach (GameObject g in playersInRange)
        {
            PlayerManager player = g.GetComponent<PlayerManager>();
            if (player.ItemPlayer.Health < lowestHP)
            {
                lowestHP = player.ItemPlayer.Health;
                lowestHPPlayer = player.gameObject;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DetectPlayerInFieldOfView(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DetectPlayerInFieldOfView(collision.gameObject);

            if (npcType == NPCType.Soldier)
            {
                FindClosestPlayer();
            }
            else if (npcType == NPCType.Turret)
            {
                FindLowestHP();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject == closestPlayer)
            {
                closestPlayer = null;
            }
            else if (collision.gameObject == lowestHPPlayer)
            {
                lowestHPPlayer = null;
            }

            if (playersInRange.Contains(collision.gameObject))
            {
                playersInRange.Remove(collision.gameObject);
            }
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //  UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, range);

        Vector3 angle1 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle2 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (angle1 * range));
        Gizmos.DrawLine(transform.position, transform.position + (angle2 * range));
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

    #endregion
}