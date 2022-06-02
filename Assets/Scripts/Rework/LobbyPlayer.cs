using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-player
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomPlayer.html
*/

/// <summary>
/// This component works in conjunction with the NetworkRoomManager to make up the multiplayer room system.
/// The RoomPrefab object of the NetworkRoomManager must have this component on it.
/// This component holds basic room player data required for the room to function.
/// Game specific data for room players can be put in other components on the RoomPrefab or in scripts derived from NetworkRoomPlayer.
/// </summary>
public class LobbyPlayer : NetworkRoomPlayer
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject[] nameHolder;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer()
    { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
    {
        RoomNetManager room = NetworkManager.singleton as RoomNetManager;
        room.lobbyPlayers.Add(this);
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient()
    {
        RoomNetManager room = NetworkManager.singleton as RoomNetManager;
        room.lobbyPlayers.Remove(this);
    }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority"/> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnection parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority()
    {
    }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority()
    { }

    #endregion Start & Stop Callbacks

    #region Room Client Callbacks

    /// <summary>
    /// This is a hook that is invoked on all player objects when entering the room.
    /// <para>Note: isLocalPlayer is not guaranteed to be set until OnStartLocalPlayer is called.</para>
    /// </summary>
    public override void OnClientEnterRoom()
    {
        RoomNetManager room = NetworkManager.singleton as RoomNetManager;
        if (!room.lobbyPlayers.Contains(this) || !room.lobbyPlayers.Any())
        {
            room.lobbyPlayers.Add(this);
        }

        UpdateDisplay();
    }

    /// <summary>
    /// This is a hook that is invoked on all player objects when exiting the room.
    /// </summary>
    public override void OnClientExitRoom()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            lobbyUI.SetActive(false);
        }
        UpdateDisplay();
    }

    #endregion Room Client Callbacks

    #region SyncVar Hooks

    /// <summary>
    /// This is a hook that is invoked on clients when the index changes.
    /// </summary>
    /// <param name="oldIndex">The old index value</param>
    /// <param name="newIndex">The new index value</param>
    public override void IndexChanged(int oldIndex, int newIndex) => UpdateDisplay();

    /// <summary>
    /// This is a hook that is invoked on clients when a RoomPlayer switches between ready or not ready.
    /// <para>This function is called when the a client player calls SendReadyToBeginMessage() or SendNotReadyToBeginMessage().</para>
    /// </summary>
    /// <param name="oldReadyState">The old readyState value</param>
    /// <param name="newReadyState">The new readyState value</param>
    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    #endregion SyncVar Hooks

    #region Optional UI

    private void UpdateDisplay()
    {
        RoomNetManager room = NetworkManager.singleton as RoomNetManager;
        if (room)
        {
            if (!hasAuthority)
            {
                foreach (var player in room.lobbyPlayers)
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
                nameHolder[i].SetActive(true);
                playerNameTexts[i].text = "Waiting For Player...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < room.roomSlots.Count; i++)
            {
                playerNameTexts[i].text = room.lobbyPlayers[i].DisplayName;
                playerReadyTexts[i].text = room.roomSlots[i].readyToBegin ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";
            }
        }
    }

    public void SetStatus()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            if (readyToBegin)
            {
                CmdChangeReadyState(false);
            }
            else
            {
                CmdChangeReadyState(true);
            }
        }
    }

    public void Disconnect()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            var manager = GameObject.Find("RoomNetManager").GetComponent<RoomNetManager>();
            manager.StopClient();
        }
    }

    public void OnGamePlay()
    {
        lobbyUI.SetActive(false);
    }

    public override void OnGUI()
    {
        base.OnGUI();
    }

    #endregion Optional UI

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        try
        {
            Debug.Log(displayName);
            if (string.IsNullOrEmpty(displayName)) displayName = "nemo";
            DisplayName = displayName;
        }
        catch (Exception e)
        {
            // show a detailed error and let the user know what went wrong
            if (e.Source.Equals("Mirror"))
            {
                Debug.LogError("OnDeserialize failed for: object=" + e);
            }
            else
            {
                Debug.LogError(e);
            }
        }
    }
}