using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to handle matchmaker-client packets
/// </summary>
public class ClientHandle : MonoBehaviour
{
    /// <summary>
    /// Function to handle sent packet from the matchmaker
    /// </summary>
    /// <param name="_packet"></param>
    public static void HandleUpdate(Packet _packet)
    {
        var entryCount = _packet.ReadInt();

        for (int i = 0; i < entryCount; i++)
        {
            // read port
            var _port = _packet.ReadInt();
            // read player count
            var _playerCount = _packet.ReadInt();
            // read max player count
            var _maxPlayers = _packet.ReadInt();
            // read server running state
            var _running = _packet.ReadBool();

            // create a new server data entry and add to ServerBrowser list
            var _newEntry = new ServerDataEntry(_port, _playerCount, _maxPlayers, _running);
            //todo: display the entry
        }
    }
}