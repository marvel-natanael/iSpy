using UnityEngine;
using Mirror;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

	See Also: NetworkManager
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

/// <summary>
/// This is a specialized NetworkManager that includes a networked room.
/// The room has slots that track the joined players, and a maximum player count that is enforced.
/// It requires that the NetworkRoomPlayer component be on the room player objects.
/// NetworkRoomManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class.
/// </summary>
public class RoomNetManager : NetworkRoomManager
{
    public List<LobbyPlayer> lobbyPlayers = new List<LobbyPlayer>();
    public GameObject[] avatars = null;
    public GameObject playerSpawner = null;

    public static event Action<NetworkConnection> OnServerReadied;

    /// <summary>
    /// Was this program was launched by matchmaker?
    /// </summary>
    private bool isMatchmakerLaunched = false;

    private bool initSent = false;

    private ushort port = 0;

    private ServerDataEntry localEntry = null;

    public override void Awake()
    {
        // Initialize Matchmaker Client
        MatchmakerClient.Singleton.Initialize();

        //Check if server is run by a matchmaker
        var args = Environment.GetCommandLineArgs();
        if (args.Contains<string>("-port"))
        {
            Debug.Log($"This Unity Server was ran by a matchmaker service!");
            isMatchmakerLaunched = true;
            try
            {
                // Get port assigned by matchmaker
                ushort newPort = ushort.Parse(args[Array.FindIndex<string>(args, m => m == "-port") + 1]);
                Debug.Log($"Unity Server port received in args : {newPort}");
                port = newPort;

                localEntry = new ServerDataEntry(newPort, maxConnections);
                Debug.Log($"Created a new local entry with port {newPort} and max player connected = {maxConnections}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        base.Awake();
    }

    public override void Start()
    {
        if (isMatchmakerLaunched)
        {
            MatchmakerClient.Singleton.Connect();
            ChangePort(port);
        }
#if UNITY_SERVER
        singleton.StartServer();
#endif
    }

    #region Server Callbacks

    /// <summary>
    /// This is called on the server when the server is started - including when a host is started.
    /// </summary>
    public override void OnRoomStartServer()
    {
        Debug.Log("Server is on");
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        // matchmaker
        if (isMatchmakerLaunched && !initSent)
        {
            ServerSend.SendInit(localEntry);
            initSent = true;
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the server when the server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnRoomStopServer()
    { }

    /// <summary>
    /// This is called on the host when a host is started.
    /// </summary>
    public override void OnRoomStartHost()
    { }

    /// <summary>
    /// This is called on the host when the host is stopped.
    /// </summary>
    public override void OnRoomStopHost()
    { }

    /// <summary>
    /// This is called on the server when a new client connects to the server.
    /// </summary>
    /// <param name="conn">The new connection.</param>
    public override void OnRoomServerConnect(NetworkConnection conn)
    {
        // matchmaker: update localEntry playersCount
        if (isMatchmakerLaunched)
        {
            localEntry.UpdateEntry(NetworkServer.connections.Count);
            ServerSend.SendUpdate(localEntry);
        }
    }

    /// <summary>
    /// This is called on the server when a client disconnects.
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        // update localEntry playersCount
        if (isMatchmakerLaunched)
        {
            localEntry.UpdateEntry(NetworkServer.connections.Count);
            ServerSend.SendUpdate(localEntry);
        }
    }

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            GameObject spawnSystemInstance = Instantiate(playerSpawner);
            NetworkServer.Spawn(spawnSystemInstance);

            // matchmaker
            if (isMatchmakerLaunched)
            {
                localEntry.UpdateEntry(true);
                ServerSend.SendUpdate(localEntry);
            }
        }

        base.OnServerSceneChanged(sceneName);
    }

    /// <summary>
    /// This allows customization of the creation of the room-player object on the server.
    /// <para>By default the roomPlayerPrefab is used to create the room-player, but this function allows that behaviour to be customized.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <returns>The new room-player object.</returns>
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    public override void OnServerError(NetworkConnection conn, Exception exception)
    {
        try
        {
            lobbyPlayers.Clear();
        }
        catch (Exception)
        {
            Debug.Log(exception.Message);
        }
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the room-player on the connection.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <param name="roomPlayer">The room player object for this connection.</param>
    /// <returns>A new GamePlayer object.</returns>
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        int index = roomPlayer.GetComponent<LobbyPlayer>().index;
        GameObject player = avatars[index];
        return player;
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>This is only called for subsequent GamePlay scenes after the first one.</para>
    /// <para>See OnRoomServerCreateGamePlayer to customize the player object for the initial GamePlay scene.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    public override void OnRoomServerAddPlayer(NetworkConnection conn)
    {
        base.OnRoomServerAddPlayer(conn);
    }

    /// <summary>
    /// This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
    /// <para>When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.</para>
    /// </summary>
    /// <param name="conn">The connection of the player</param>
    /// <param name="roomPlayer">The room player object.</param>
    /// <param name="gamePlayer">The game player object.</param>
    /// <returns>False to not allow this player to replace the room player.</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        var lobbyPlayer = roomPlayer.GetComponent<LobbyPlayer>();
        if (lobbyPlayer)
        {
            lobbyPlayer.OnGamePlay();
        }
        return true;
    }

    /// <summary>
    /// This is called on the server when all the players in the room are ready.
    /// <para>The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the room are ready, such as adding a countdown or a confirmation for a group leader.</para>
    /// </summary>
    public override void OnRoomServerPlayersReady()
    {
        base.OnRoomServerPlayersReady();
    }

    /// <summary>
    /// This is called on the server when CheckReadyToBegin finds that players are not ready
    /// <para>May be called multiple times while not ready players are joining</para>
    /// </summary>
    public override void OnRoomServerPlayersNotReady()
    { }

    public void ResetGame()
    {
        ServerChangeScene(RoomScene);
        if (isMatchmakerLaunched)
        {
            localEntry.UpdateEntry(false);
            ServerSend.SendUpdate(localEntry);
        }
    }

    #endregion Server Callbacks

    #region Client Callbacks

    /// <summary>
    /// This is a hook to allow custom behaviour when the game client enters the room.
    /// </summary>
    public override void OnRoomClientEnter()
    { }

    /// <summary>
    /// This is a hook to allow custom behaviour when the game client exits the room.
    /// </summary>
    public override void OnRoomClientExit()
    { }

    /// <summary>
    /// This is called on the client when it connects to server.
    /// </summary>
    public override void OnRoomClientConnect()
    { }

    /// <summary>
    /// This is called on the client when disconnected from a server.
    /// </summary>
    public override void OnRoomClientDisconnect()
    {
    }

    /// <summary>
    /// This is called on the client when a client is started.
    /// </summary>
    public override void OnRoomStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    /// <summary>
    /// This is called on the client when the client stops.
    /// </summary>
    public override void OnRoomStopClient()
    { }

    /// <summary>
    /// This is called on the client when the client is finished loading a new networked scene.
    /// </summary>
    public override void OnRoomClientSceneChanged()
    { }

    /// <summary>
    /// Called on the client when adding a player to the room fails.
    /// <para>This could be because the room is full, or the connection is not allowed to have more players.</para>
    /// </summary>
    public override void OnRoomClientAddPlayerFailed()
    { }

    #endregion Client Callbacks

    #region Optional UI

    public override void OnGUI()
    {
        base.OnGUI();
    }

    #endregion Optional UI

    #region Matchmaker Stuff

    /// <summary>
    /// Change the port for the current used transport
    /// </summary>
    /// <param name="_port">new port</param>
    public static void ChangePort(int _port)
    {
        if (!singleton.gameObject.GetComponent<kcp2k.KcpTransport>()) { Debug.LogError($"Transport doesn't exist, cannot change port!"); return; }
        if (_port > ushort.MaxValue & _port <= 0) { Debug.LogError($"port invalid"); return; }
        singleton.gameObject.GetComponent<kcp2k.KcpTransport>().Port = (ushort)_port;
        Debug.Log($"Transport's port is now {singleton.GetComponent<kcp2k.KcpTransport>().Port}");
    }

    public override void OnApplicationQuit()
    {
        if (MatchmakerClient.Singleton.transport != null && MatchmakerClient.Singleton.IsConnected)
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                MatchmakerClient.Singleton.Disconnect();
            });
            Debug.Log("quit");
        }
        base.OnApplicationQuit();
    }

    [ContextMenu(nameof(TestConnect))]
    private void TestConnect()
    {
    }

    #endregion Matchmaker Stuff
}