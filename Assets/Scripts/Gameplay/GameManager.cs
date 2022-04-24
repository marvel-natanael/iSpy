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

    [SerializeField]
    int pCount;

    public void Hook_GameOver(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            if (isClient)
            {
                //StartCoroutine(ClearRoom());
                var manager = NetworkManager.singleton as LobbyNetworkManager;
                Debug.Log("STOP CLIENT");
                manager.StopClient();
            }
        }
    }
    void OnEnable()
    {
        PlayerManager.OnGameOver += GameOver;
    }
    void OnDisable()
    {
        PlayerManager.OnGameOver -= GameOver;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //pCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void Update()
    {
        pCount = GameObject.FindGameObjectsWithTag("Player").Length;
        if(pCount == 0)
        {
            if(!gameOver)
            {
                GameOver();
            }
        }
        //StartCoroutine(ClearRoom());
        //yield return new WaitForSeconds(2f);

        /*if (gameOver)
        {
            var manager = NetworkManager.singleton as LobbyNetworkManager;

            //StartCoroutine(ClearRoom());
            if (isClient)
            {
                Debug.Log("STOP CLIENT");
                manager.StopClient();
            }

            else if (isServer)
            {
                Debug.Log("Conn Count : " + NetworkServer.connections.Count);
                if (NetworkServer.connections.Count <= 0)
                {
                    manager.ResetGame();
                    //var lobbyScene = NetworkManager.singleton.onlineScene;
                    //SceneManager.LoadScene(lobbyScene);
                }
            }
        }*/
    }


    public void GameOver()
    {
        if (playersCount <= 1 || pCount == 1)
        {
            gameOver = true;
            var manager = NetworkManager.singleton as LobbyNetworkManager;

            //StartCoroutine(ClearRoom());

            if (isServer)
            {
                Debug.Log("Conn Count : " + NetworkServer.connections.Count);
                //if (NetworkServer.connections.Count <= 0)
                //{
                manager.RoomPlayers.Clear();
                manager.GamePlayers.Clear();
                manager.ResetGame();
                //var lobbyScene = NetworkManager.singleton.onlineScene;
                //SceneManager.LoadScene(lobbyScene);
                //}
            }

            //if (isServer)
            //{
            //    StartCoroutine(ServerReturnToLobbyCoroutine(2f));
            //}
        }
    }

    private IEnumerator ClearRoom()
    {/*
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        */
        yield return new WaitForSeconds(2f);

        /*        if (gameOver)
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
                }*/
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
