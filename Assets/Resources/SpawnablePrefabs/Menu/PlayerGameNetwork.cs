/*using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerGameNetwork : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";
    [SerializeField]
    Image loadingPanel;

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
        Room.changedScenePlayers++;
    }
    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    public void CmdSceneChanged()
    {
        loadingPanel.gameObject.SetActive(false);
    }

    [Server]
    public void SetDisplayName(string name)
    {
        displayName = name;
    }
}
*/