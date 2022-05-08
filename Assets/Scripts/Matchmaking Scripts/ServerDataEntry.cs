using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataEntry : MonoBehaviour
{
    private ushort port;
    private int playerCount;
    private int maxPlayer;
    private bool running;

    public int MaxPlayer => maxPlayer;
    public int PlayerCount => playerCount;
    public ushort Port => port;
    public bool Running => running;

    public ServerDataEntry(int _port, int _playerCount, int _maxPlayer, bool _running)
    {
        port = (ushort)_port;
        playerCount = _playerCount;
        maxPlayer = _maxPlayer;
        running = _running;
    }

    public ServerDataEntry(ushort newPort, int maxPlayer)
    {
        port = newPort;
        playerCount = 0;
        maxPlayer = 0;
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