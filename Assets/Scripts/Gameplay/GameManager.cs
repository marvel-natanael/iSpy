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
    public int playersCount = 2, returningPlayer;
    [SyncVar(hook = nameof(Hook_GameOver))]
    public bool gameOver = false;
    [SyncVar]
    public List<string> playerNames = new List<string>();
    [SyncVar(hook =nameof(Hook_WinnerNameFound))]
    public string winnerName = "Loading...";

    [SerializeField]
    int pCount;
    [SerializeField]
    bool counting, starting;
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    TextMeshProUGUI winText;

    [SerializeField] private GameObject losePanel;

    [SyncVar]
    public List<GameObject> players = new List<GameObject>();

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(CountPlayers());
        //InvokeRepeating("getPlayersPos", 5f, 3f);
    }

    [ServerCallback]
    void getPlayersPos()
    {
        if(players == null) { return; }
        foreach(GameObject player in players)
        {
            if(player != null)
            Debug.Log(player.name + player.transform.position);
        }
    }

    private void LateUpdate()
    {
        pCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }
    private void Update()
    {
        if (pCount <= 1 && starting)
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

        if (isClient && pCount <= 1)
        {
            if(!winPanel.activeInHierarchy)
            winPanel.SetActive(true);
            
           losePanel.SetActive(false);
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

        yield return new WaitForSeconds(4f);
        if (isServer)
        {
            var manager = GameObject.Find("RoomNetManager").GetComponent<RoomNetManager>();
            manager.lobbyPlayers.Clear();

            pCount = GameObject.FindGameObjectsWithTag("Player").Length;
            starting = true;
        }
    }

    private void ServerReturnToLobbyCoroutine()
    {
        var manager = GameObject.Find("RoomNetManager").GetComponent<RoomNetManager>();
        if (!isServer) { return; }

        //manager.lobbyPlayers.Clear();
        //manager.lobbyPlayers.TrimExcess();
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

    public void LeaveGame()
    {
        Debug.Log("Test 1");
        var manager = GameObject.Find("RoomNetManager").GetComponent<RoomNetManager>();
        if (isServer) { return; }
        Debug.Log("Test 2");
        manager.StopClient();
        
        Debug.Log("Test 3");
    }
    
}