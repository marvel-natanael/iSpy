using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataEntry : MonoBehaviour
{
    private ushort port;
    private int playerCount;
    private bool running;

    public int PlayerCount => playerCount;
    public ushort Port => port;
    public bool Running => running;

    public ServerDataEntry(ushort newPort)
    {
        port = newPort;
        playerCount = 0;
        running = false;
    }

    public void UpdateEntry(int newCount)
    {
        playerCount = newCount;
    }

    public void UpdateEntry(bool isRunning)
    {
        running = isRunning;
    }
}