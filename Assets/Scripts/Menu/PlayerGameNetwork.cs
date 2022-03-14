using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerGameNetwork : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    private LobbyNetworkManager room;
    private LobbyNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as LobbyNetworkManager;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this); 
    }
    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }
    [Server]
    public void SetDisplayName(string name)
    {
        displayName = name;
    }
}
