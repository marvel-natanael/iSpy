using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayerRoomNetwork : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private Canvas canvas;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private LobbyNetworkManager room;
    private LobbyNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as LobbyNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.displayName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            canvas.sortingOrder = 4 - i;
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
        //Room.NotifyPlayersOfReadyState();
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!readyToStart) { return; }
        CmdStartGame();

/*        Debug.Log(readyToStart);
        switch (readyToStart)
        {
            case "not enough player!":
                {
                    break;
                }
            case "all set":
                {
                    CmdStartGame();
                    break;
                }
            default:
                {
                    Debug.Log(readyToStart);
                    break;
                }
        }*/
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        //if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }
        Room.StartGame();
    }

    public void LeaveRoom()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Room.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            Room.StopClient();
        }
    }
}