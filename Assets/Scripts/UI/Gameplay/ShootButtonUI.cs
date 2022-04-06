using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ShootButtonUI : MonoBehaviour
{
    PlayerShoot targetPlayer;

    public void SetTargetPlayer(PlayerShoot player)
    {
        gameObject.SetActive(true);
        targetPlayer = player;
    }

    public void OnClickButtonShoot()
    {
        targetPlayer.Shoot();
    }
}
