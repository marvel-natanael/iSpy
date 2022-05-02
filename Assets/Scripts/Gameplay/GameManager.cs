using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Player;
using TMPro;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    [SyncVar]
    public int playersCount, returningPlayer;
    [SyncVar(hook = nameof(Hook_GameOver))]
    public bool gameOver = false;
    [SyncVar]
    public List<string> playerNames = new List<string>();
    [SyncVar(hook =nameof(Hook_WinnerNameFound))]
    public string winnerName = "Loading";

    [SerializeField]
    int pCount;
    [SerializeField]
    bool counting, starting;
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    TextMeshProUGUI winText;

    public void Hook_GameOver(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            if (isClient)
            {
                Debug.Log("Panel Activated");
                winPanel.SetActive(true);
            }
        }
    }

    public void Hook_WinnerNameFound(string oldVal, string newVal)
    {
        if(newVal != null)
        {
            Debug.Log(newVal);
            if(isClient)
            {
                try
                {
                    winText.text = newVal;
                }
                catch(Exception e)
                {
                    // show a detailed error and let the user know what went wrong
                    if (e.Source.Equals("Mirror"))
                    {
                        Debug.Log("X");
                    }
                    else
                    {
                        Debug.LogError(e);
                    }
                }
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
        StartCoroutine("CountPlayers");
    }

    private void Update()
    {
        pCount = GameObject.FindGameObjectsWithTag("Player").Length;

        if (pCount <= 1)
        {
            if (!gameOver) GameOver();
            //StartCoroutine(ClearRoom());   
        }
        if (returningPlayer == pCount && returningPlayer != 0 || pCount == 0 && starting)
        {
            if(!counting) ServerReturnToLobbyCoroutine();
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
        //var manager = NetworkManager.singleton as LobbyNetworkManager;

        if (isClient)
        {
            if(!winPanel.activeInHierarchy)
            winPanel.SetActive(true);
        }
        if (isServer)
        {
            Debug.Log("Conn Count : " + NetworkServer.connections.Count);

            var winner = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            if(winner != null)
            {
                winnerName = winner.playerName;
            }
        }
    }

    private IEnumerator CountPlayers()
    {/*
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        */
        if(isServer)
        {
            yield return new WaitForSeconds(4f);
            pCount = GameObject.FindGameObjectsWithTag("Player").Length;
            starting = true;
        }
    }

    private void ServerReturnToLobbyCoroutine()
    {
        var manager = NetworkManager.singleton as LobbyNetworkManager;
        if (!isServer) { return; }

        manager.RoomPlayers.Clear();
        manager.GamePlayers.Clear();
        manager.ResetGame();
        counting = true;
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