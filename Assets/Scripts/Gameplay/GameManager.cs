using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Player;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    [SyncVar]
    public int playersCount;
    [SyncVar(hook = nameof(Hook_GameOver))]
    public bool gameOver = false;

    public void Hook_GameOver(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            Debug.Log("GAME OVER");
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }


    public void GameOver()
    {
        if(playersCount == 1)
        {
            gameOver = true;
        }
    }
}
