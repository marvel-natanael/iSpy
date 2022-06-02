using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class BrowserConnScript : MonoBehaviour
{
    [SerializeField] private GameObject failToConnectLabel;
    [SerializeField] private TextMeshProUGUI connLabel;
    [SerializeField] private Button switchButton;
    [SerializeField] private Button connButton;

    [SerializeField] private string hostAddress;

    private bool isForeign = false;

    private void Awake()
    {
        connLabel.text = "local";
        connLabel.color = Color.red;
    }

    private void Start()
    {
        MatchmakerClient.OnMClientConnected += OnClientConnected;
        MatchmakerClient.OnMClientDisconnected += OnClientDisconnected;
        MatchmakerClient.OnMCFailedToConnect += OnClientFailToConnect;
    }

    private void OnClientFailToConnect()
    {
        Instantiate(failToConnectLabel, transform);
    }

    private void OnClientDisconnected()
    {
        connLabel.color = Color.red;
        switchButton.interactable = true;
        connButton.interactable = true;
    }

    private void OnClientConnected()
    {
        connLabel.color = Color.green;
        switchButton.interactable = false;
        connButton.interactable = false;
    }

    /// <summary>
    /// Button function: when clicked, toggle connection between local and foreign
    /// </summary>
    public void B_ChangeHost()
    {
        isForeign = !isForeign;
        if (isForeign)
        {
            connLabel.text = "AWS Server";
            NetworkManager.singleton.networkAddress = hostAddress;
            return;
        }
        NetworkManager.singleton.networkAddress = "127.0.0.1";
        connLabel.text = "Local";
    }

    /// <summary>
    /// Button function: when clicked, connect to the selected host
    /// </summary>
    public void B_ConnectToHost()
    {
        if (isForeign)
        {
            MatchmakerClient.Singleton.Connect(hostAddress, 7777);
        }
        else MatchmakerClient.Singleton.Connect("127.0.0.1", 7777);
    }

    private void OnDestroy()
    {
        MatchmakerClient.OnMClientConnected -= OnClientConnected;
        MatchmakerClient.OnMClientDisconnected -= OnClientDisconnected;
        MatchmakerClient.OnMCFailedToConnect -= OnClientFailToConnect;
    }
}