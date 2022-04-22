using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Player;
using UnityEngine.SceneManagement;

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
            if (isClient)
            {
                //StartCoroutine(ClearRoom());
            }
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

    private void Update()
    {
        StartCoroutine(ClearRoom());
    }


    public void GameOver()
    {
        if(playersCount == 1)
        {
            gameOver = true;

            //if (isServer)
            //{
            //    StartCoroutine(ServerReturnToLobbyCoroutine(2f));
            //}
        }
    }

    private IEnumerator ClearRoom()
    {
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        
        yield return new WaitForSeconds(2f);
        
        if (gameOver)
        {
            if (isClient)
            {
                Debug.Log("STOP CLIENT");
                manager.StopClient();
            }

            else if (isServer)
            {
                Debug.Log("Conn Count : " + NetworkServer.connections.Count);
                if(NetworkServer.connections.Count <= 0)
                {
                    manager.ResetGame();
                    //var lobbyScene = NetworkManager.singleton.onlineScene;
                    //SceneManager.LoadScene(lobbyScene);
                }
            }
        }
    }

    private IEnumerator ServerReturnToLobbyCoroutine(float delay)
    {
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        yield return new WaitForSeconds(delay);
        manager.ResetGame();
    }

    private void ServerReturnToLobby()
    {
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        manager.ResetGame();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        Debug.Log("ON STOP CLIENT CALLED ");
    }
}
