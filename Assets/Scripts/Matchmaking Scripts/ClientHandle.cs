using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to handle matchmaker-client packets
/// </summary>
public class ClientHandle
{
    public static void HandleInit(Packet _packet)
    {
        // read entry count
        var entryCount = _packet.ReadInt();

        for (int i = 0; i < entryCount; i++)
        {
            // read port
            var _port = _packet.ReadInt();
            // read max player count
            var _mPlrCount = _packet.ReadInt();
            // read player count
            var _plrCount = _packet.ReadInt();
            // read running state
            var _rState = _packet.ReadBool();

            ServerEntries.Singleton.SetData(new ServerDataEntry(_port, _mPlrCount, _plrCount, _rState));
        }
    }

    /// <summary>
    /// Function to handle update packet from the matchmaker
    /// </summary>
    /// <param name="_packet">packet to be read</param>
    public static void HandleUpdate(Packet _packet)
    {
        // read updated count
        var _count = _packet.ReadInt();

        for (int i = 0; i < _count; i++)
        {
            // read which port is updated
            var _port = _packet.ReadInt();
            // read player count
            var _playerCount = _packet.ReadInt();
            // read server running state
            var _running = _packet.ReadBool();

            // update server entry
            ServerEntries.Singleton.UpdateData(_port, _playerCount, _running);
        }
    }
}