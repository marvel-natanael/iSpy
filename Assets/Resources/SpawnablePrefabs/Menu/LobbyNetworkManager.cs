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

    public static event Action OnClientConnected;

    public static event Action OnClientDisconnected;

    public static event Action<NetworkConnection> OnServerReadied;

    public static event Action OnServerStopped;

    //Matchmaker variabes
#if UNITY_SERVER

#endif

    private ServerDataEntry serverData;
    private bool isMatchmakerLaunched = false;

    public override void Awake()
    {
#if UNITY_SERVER
        //Check if we're using kcp2k
        if (transport.GetType() == typeof(kcp2k.KcpTransport))
        {
            //Check if server is run by a matchmaker
            var args = Environment.GetCommandLineArgs();
            if (args.Contains<string>("-port"))
            {
                try
                {
                    ushort newPort = ushort.Parse(args[Array.FindIndex<string>(args, m => m == "-port") + 1]);
                    GetComponent<kcp2k.KcpTransport>().Port = newPort;
                    isMatchmakerLaunched = true;

                    serverData = new ServerDataEntry(newPort, maxConnections);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Argument port is invalid {e}");
                }
            }
        }
#endif
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

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
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

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

        //Do a recount of connected clients, update server data
        if (isMatchmakerLaunched)
        {
            serverData.UpdateEntry(NetworkServer.connections.Count);
            ServerSend.SendUpdate(serverData);
        }

        //RoomPlayers.Add(conn.identity.gameObject.GetComponent<PlayerRoomNetwork>());
    }


    public override void OnStopClient()
    {
        base.OnStopClient();
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

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
        Debug.Log(conn.connectionId);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
    }

    public static string GetAddress()
    {
        return singleton.networkAddress;
    }

    public static void ChangePort(int _port)
    {
        if (_port < ushort.MaxValue & _port > 0)
            singleton.GetComponent<kcp2k.KcpTransport>().Port = (ushort)_port;
    }
}