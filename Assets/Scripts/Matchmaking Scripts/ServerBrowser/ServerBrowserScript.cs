using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ServerBrowserScript : MonoBehaviour
{
    public static ServerBrowserScript Singleton;
    private readonly List<GameObject> contentList = new List<GameObject>();

    public EntryObject CurrentSelected;
    public GameObject EntryButtonPrefab;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Debug.LogWarning($"ServerBrowserScript already existed, destorying...");
            Destroy(this);
        }
    }

    private void Start()
    {
        ServerEntries.OnDatabaseUpdate += OnDatabaseUpdate;
    }

    /// <summary>
    /// Called when <c>ServerEntries.Singleton.Database gets updated</c>
    /// </summary>
    private void OnDatabaseUpdate()
    {
        // I know I should find a way to only update the updated ones, but I am running out of time so I just reset everything
        ResetAllData();

        // get server browser content
        var scrollRect = GetComponent<ScrollRect>();

        // foreach entries in the database, make an entry object
        foreach (var _entry in ServerEntries.Singleton.Database)
        {
            var entryObj = Instantiate(EntryButtonPrefab, scrollRect.content).GetComponent<EntryObject>();
            entryObj.SetPortID(_entry.Value.Port);
            entryObj.UpdateData(_entry.Value.Port, _entry.Value);

            // add entry object to list of entry objects
            contentList.Add(entryObj.gameObject);
        }
    }

    /// <summary>
    /// Destroys every entry object in <c>contentList</c> and clearing it
    /// </summary>
    private void ResetAllData()
    {
        CurrentSelected = null;
        foreach (var entry in contentList)
        {
            Destroy(entry);
        }
        contentList.Clear();
    }

    /// <summary>
    /// Button function: when clicked, connect to selected server
    /// </summary>
    public void B_ConnectToSelected()
    {
        if (ConnectChecks())
        {
            RoomNetManager.ChangePort(CurrentSelected.PortID);
            MatchmakerClient.Singleton.Disconnect();
            NetworkManager.singleton.StartClient();
        }
    }

    /// <summary>
    /// Button function: when clicked, request the latest update manually
    /// </summary>
    public void B_RequestUpdate()
    {
        if (MatchmakerClient.Singleton.IsConnected)
        {
            ClientSend.SendUpdateRequest();
        }
    }

    /// <summary>
    /// Do checks if connecting is viable
    /// </summary>
    /// <returns><see langword="true"/>, if connecting is doable</returns>
    private bool ConnectChecks()
    {
        if (!CurrentSelected)
        {
            return false;
        }
        return true;
    }
}