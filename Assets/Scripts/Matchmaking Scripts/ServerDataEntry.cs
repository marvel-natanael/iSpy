using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataEntry
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

    public ServerDataEntry(int newPort, int maxPlayer)
    {
        port = (ushort)newPort;
        playerCount = 0;
        maxPlayer = 0;
        running = false;
    }

    /// <summary>
    /// Updates <c>playerCount</c> field
    /// </summary>
    /// <param name="newCount"></param>
    public void UpdateEntry(int newCount)
    {
        playerCount = newCount;
        Debug.Log($"playerCount entry for {port} is updated");
    }

    /// <summary>
    /// Updates <c>running</c> field
    /// </summary>
    /// <param name="isRunning">new state</param>
    public void UpdateEntry(bool isRunning)
    {
        running = isRunning;
        Debug.Log($"running entry for {port} is updated");
    }

    /// <summary>
    /// Updates both <c>playercount</c> and <c>running</c> fields
    /// </summary>
    /// <param name="newCount">new player count</param>
    /// <param name="isRunning">new running state</param>
    public void UpdateEntry(int newCount, bool isRunning)
    {
        playerCount = newCount;
        running = isRunning;
        Debug.Log($"playerCount and running entry for {port} is updated");
    }
}