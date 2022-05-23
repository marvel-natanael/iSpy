using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Database handling
/// </summary>
public class ServerEntries
{
    private static ServerEntries singleton;
    private List<ServerDataEntry> serverList = new List<ServerDataEntry>();

    /// <summary>
    /// The one and only entry database
    /// </summary>
    public static ServerEntries Singleton
    { get { if (singleton == null) singleton = new ServerEntries(); return singleton; } }

    /// <summary>
    /// The main database, contains the latest updated state for every server
    /// </summary>
    public List<ServerDataEntry> Database => serverList;

    public static event Action OnDatabaseUpdate;

    /// <summary>
    /// Inserts a new entry in the database
    /// </summary>
    /// <param name="entry">new entry</param>
    public void SetData(ServerDataEntry entry)
    {
        // check if the entry doesn't exist
        if (serverList.Contains(entry)) { Debug.LogWarning($"Duplicate entry, destroying"); return; }

        // add the entry to server list
        serverList.Add(entry);
        OnDatabaseUpdate?.Invoke();
    }

    /// <summary>
    /// Updates a data entry in the database
    /// </summary>
    /// <param name="index">entry index</param>
    /// <param name="_PlayerCount">entry's new player count</param>
    /// <param name="_isRunning">entry's new running state</param>
    public void UpdateData(int index, int _PlayerCount, bool _isRunning)
    {
        serverList[index].UpdateEntry(_PlayerCount);
        serverList[index].UpdateEntry(_isRunning);

        OnDatabaseUpdate?.Invoke();
    }
}