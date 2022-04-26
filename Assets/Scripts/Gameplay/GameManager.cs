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
    public int playersCount, returningPlayer;
    [SyncVar(hook = nameof(Hook_GameOver))]
    public bool gameOver = false;

    [SerializeField]
    int pCount;
    [SerializeField]
    bool counting;
    [SerializeField]
    GameObject winPanel;
    public void Hook_GameOver(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            if (isClient)
            {
                //StartCoroutine(ClearRoom());
                var manager = NetworkManager.singleton as LobbyNetworkManager;
                Debug.Log("STOP CLIENT");
                //manager.StopClient();
                winPanel.SetActive(true);
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
        //StartCoroutine("CountPlayers");
    }

    private void Update()
    {
        pCount = GameObject.FindGameObjectsWithTag("Player").Length;

        if (pCount < 2)
        {
            if (!gameOver) GameOver();
            //StartCoroutine(ClearRoom());   
        }
        if (returningPlayer == pCount)
        {
            if (!counting) ServerReturnToLobbyCoroutine();
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
        gameOver = true;
        var manager = NetworkManager.singleton as LobbyNetworkManager;

        if(isClient)
        {
            winPanel.SetActive(true);
        }
        if (isServer)
        {
            Debug.Log("Conn Count : " + NetworkServer.connections.Count);
            //if (NetworkServer.connections.Count <= 0)
            //{
            //manager.RoomPlayers.Clear();
            //manager.GamePlayers.Clear();
            //manager.ResetGame();
            //var lobbyScene = NetworkManager.singleton.onlineScene;
            //SceneManager.LoadScene(lobbyScene);
            //}
        }
    }

    private IEnumerator CountPlayers()
    {/*
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        */
        yield return new WaitForSeconds(4f);
        pCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void ServerReturnToLobbyCoroutine()
    {
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        if(isServer)
        {
            manager.RoomPlayers.Clear();
            manager.GamePlayers.Clear();
            manager.ResetGame();
            counting = true;
        }
    }
    [Command(requiresAuthority = false)]
    public void ReturnToLobby()
    {
        //var manager = NetworkManager.singleton as LobbyNetworkManager;
        //manager.ResetGame();
        instance.returningPlayer++;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        Debug.Log("ON STOP CLIENT CALLED ");
    }
}