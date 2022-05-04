using System;
using Mirror;
using UnityEngine;

public class RotateWeaponTurret : NetworkBehaviour
{
    [SerializeField] private DetectionPlayer _detectionPlayer;

    private void Update()
    {
        if (_detectionPlayer.detection)
        {
            var direction = GameObject.FindGameObjectsWithTag("Player")[0].transform.position - transform.position;

            var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
        }
    }
}