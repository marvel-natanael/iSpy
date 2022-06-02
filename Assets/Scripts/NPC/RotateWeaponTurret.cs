using System;
using Mirror;
using UnityEngine;

public class RotateWeaponTurret : MonoBehaviour
{
    [SerializeField] private DetectionPlayer _detectionPlayer;

    private float nearDistance;

    private void Start()
    {
        nearDistance = _detectionPlayer.GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
       
        var target = TargetPlayer();

        if (target)
        {
            var direction = target.transform.position - transform.position;

            var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
        }
        else
        {
            nearDistance = _detectionPlayer.GetComponent<CircleCollider2D>().radius;
        }
    }

    private GameObject TargetPlayer()
    {
        var playerList = _detectionPlayer.GetListPlayer();
        
        foreach(var p in playerList)
        {
            float dist = (transform.position - p.transform.position).magnitude;
            if (dist < nearDistance)
            {
                nearDistance = dist;
                return p;
            }
        }

        return null;
    }
}