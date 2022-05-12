using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Database handling
/// </summary>
public class ServerEntries : MonoBehaviour
{
    public static ServerEntries Singleton;

    private List<ServerDataEntry> serverList = new List<ServerDataEntry>();

    /// <summary>
    /// The main database, contains the latest updated state for every server
    /// </summary>
    public List<ServerDataEntry> Database => serverList;

    public static event Action onDatabaseUpdate;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else if (Singleton != this)
        {
            Debug.LogWarning($"Multiple server database detected, destroying...");
            Destroy(this);
        }
    }

    public void SetData(ServerDataEntry entry)
    {
        // check if the entry doesn't exist
        if (!serverList.Contains(entry))
        {
            // add the entry to server list
            serverList.Add(entry);
        }

        onDatabaseUpdate?.Invoke();
    }

    public void UpdateData(int index, int _PlayerCount, bool _isRunning)
    {
        serverList[index].UpdateEntry(_PlayerCount);
        serverList[index].UpdateEntry(_isRunning);

        onDatabaseUpdate?.Invoke();
    }
}