using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerBrowserScript : MonoBehaviour
{
    private List<ServerDataEntry> serverList;
    private ServerDataEntry currentSelected;

    public static event Action onServerUpdated;

    public void AddEntryToList(ServerDataEntry newEntry)
    {
        serverList.Add(newEntry);
        onServerUpdated?.Invoke();
    }

    public void RemoveEntryFromList(int id)
    {
        serverList.Remove(serverList.Find(match => match.Port == id));
        onServerUpdated?.Invoke();
    }

    public void Connect()
    {
    }
}