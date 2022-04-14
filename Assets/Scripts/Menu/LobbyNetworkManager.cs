using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class LobbyNetworkManager : NetworkManager
{
    [SerializeField]
    private int minPlayers = 1;

    [Scene] [SerializeField] private string menuScene = string.Empty;

    [SerializeField] private PlayerRoomNetwork playerRoomPrefab = null;
    [SerializeField] private PlayerGameNetwork playerGamePrefab = null;
    [SerializeField] private GameObject playerSpawner = null;

    public static event Action OnClientConnected;

    public static event Action OnClientDisconnected;

    public static event Action<NetworkConnection> onServerReadied;

    public static event Action OnServerStopped;

    public List<PlayerRoomNetwork> RoomPlayers { get; } = new List<PlayerRoomNetwork>();
    public List<PlayerGameNetwork> GamePlayers { get; } = new List<PlayerGameNetwork>();

    private bool isCountdown = false;

    private bool isStartGame = false;

    public bool IsStartGame
    { get { return isStartGame; } }

    public override void Start()
    {
#if UNITY_SERVER
        if (transport.GetType() == typeof(TelepathyTransport))
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Contains<string>("-port"))
            {
                ushort newPort = ushort.Parse(args[Array.FindIndex<string>(args, m => m == "-port") + 1]);
                GetComponent<TelepathyTransport>().port = newPort;
            }
        }
#endif
        base.Start();
    }

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect(NetworkClient.connection);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect(NetworkClient.connection);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }

        //RoomPlayers.Add(conn.identity.gameObject.GetComponent<PlayerRoomNetwork>());
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            //bool isLeader = RoomPlayers.Count == 0;
            PlayerRoomNetwork roomPlayerInstance = Instantiate(playerRoomPrefab);
            //roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<PlayerRoomNetwork>();
            RoomPlayers.Remove(player);
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        if (SceneManager.GetActiveScene().path == onlineScene)
        {
            var lobby = GameObject.Find("MenuManager").GetComponent<MenuUIManager>();
            lobby.ShowMainMenu();
        }
    }

    public void NotifyPlayersOfReadyState()
    {
        isStartGame = IsReadyToStart();
        StartCoroutine(StartGame(isStartGame));
        //foreach (var player in RoomPlayers)
        //{
        //    player.HandleReadyToStart(IsReadyToStart());
        //
        //}
    }

    private IEnumerator StartGame(bool isStartable)
    {
        yield return new WaitForSeconds(.1f);
        if (isStartable)
        {
            StartGame();
        }
    }

    //public string IsReadyToStart()
    //{
    //    Debug.Log("Roomplayer count : " + RoomPlayers.Count);
    //
    //    if (RoomPlayers.Count < minPlayers) { return "not enough player!"; }
    //
    //    Debug.Log("Roomplayer count : " + RoomPlayers.Count);
    //
    //    foreach (var player in RoomPlayers)
    //    {
    //        if (!player.IsReady) { return "not ready"; }
    //    }
    //
    //    return "all set";
    //}

    private bool IsReadyToStart()
    {
        //Debug.Log("Numplayer : " + numPlayers);

        int numberOfReadyPlayers = NetworkServer.connections.Count(conn => conn.Value != null && conn.Value.identity.gameObject.GetComponent<PlayerRoomNetwork>().IsReady);
        if (numberOfReadyPlayers < minPlayers) { return false; }

        //Debug.Log("Roomplayer count : " + RoomPlayers.Count);

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) { return; }
            ServerChangeScene("Map");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Map"))
        {
            foreach (GameObject prn in GameObject.FindGameObjectsWithTag("RoomPlayer"))
            {
                var prngo = prn.GetComponent<PlayerRoomNetwork>();
                RoomPlayers.Add(prngo);
            }
            Debug.Log(RoomPlayers.Count);
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                PlayerGameNetwork gamePlayerInstance = Instantiate(playerGamePrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Map"))
        {
            GameObject spawnSystemInstance = Instantiate(playerSpawner);
            NetworkServer.Spawn(spawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        onServerReadied?.Invoke(conn);
        Debug.Log(conn.connectionId);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
        RoomPlayers.Clear();
        GamePlayers.Clear();
    }
}