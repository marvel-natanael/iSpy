using System.Collections;
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
    }
    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    public void CmdSceneChanged()
    {
        loadingPanel.gameObject.SetActive(false);
    }

    IEnumerator DisableLoading()
    {
        float duration = 3f; 
        float timer = 0;
        while (duration >= timer)
        {
            duration -= Time.deltaTime;
            var text = loadingPanel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = duration.ToString();
            yield return null;
        }
    }

    [Server]
    public void SetDisplayName(string name)
    {
        displayName = name;
    }
}
