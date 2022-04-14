using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataEntry
{
    private ushort playerCount;
    private ushort port;

    public ushort PlayerCount => playerCount;
    public ushort Port => port;

    public ServerDataEntry()
    {
        playerCount = 0;
        port = 0;
    }

    public ServerDataEntry(ushort newCount, ushort newPort)
    {
        playerCount = newCount;
        port = newPort;
    }
}