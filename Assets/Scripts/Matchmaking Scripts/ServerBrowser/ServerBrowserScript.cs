using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ServerBrowserScript : MonoBehaviour
{
    public static ServerBrowserScript singleton;
    public static EntryObject CurrentSelected { get => singleton.currentSelected; set => singleton.currentSelected = value; }

    private EntryObject currentSelected;
    private List<GameObject> contentList = new List<GameObject>();
    private List<ServerDataEntry> serverList = new List<ServerDataEntry>();

    public GameObject EntryButtonPrefab;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Debug.LogWarning($"ServerBrowserScript already existed, destorying...");
            Destroy(this);
        }
    }

    public void B_ConnectToSelected()
    {
        LobbyNetworkManager.ChangePort(serverList[CurrentSelected.id].Port);
        NetworkClient.Connect(LobbyNetworkManager.GetAddress());
        MatchmakerClient.instance
    }

    public static void SetData(ServerDataEntry entry)
    {
        // check if the entry doesn't exist
        if (!singleton.serverList.Contains(entry))
        {
            // add the entry to server list
            singleton.serverList.Add(entry);

            // create new object for browser
            var newEntryObj = Instantiate(singleton.EntryButtonPrefab, singleton.GetComponent<ScrollRect>().content);
            newEntryObj.GetComponent<EntryObject>().UpdateData(entry, singleton.serverList.Count);
            singleton.contentList.Add(newEntryObj);
        }
    }

    private void ResetAllData()
    {
        currentSelected = null;
        contentList = new List<GameObject>();
        serverList = new List<ServerDataEntry>();
    }
}