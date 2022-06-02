using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;

public class ShootButtonUI : MonoBehaviour
{
    PlayerShoot targetPlayer;
    [SerializeField] private Button btnShoot;

    public void SetTargetPlayer(PlayerShoot player)
    {
        gameObject.SetActive(true);
        targetPlayer = player;
    }

    public void OnClickButtonShoot()
    {
        targetPlayer.Shoot();
    }

    private void Update()
    {
        if (btnShoot != null && targetPlayer != null)
        {
            btnShoot.image.color = targetPlayer.GetShoot()? new Color32(255, 255, 225, 255) : new Color32(55, 55, 55, 255);
        }
    }
}