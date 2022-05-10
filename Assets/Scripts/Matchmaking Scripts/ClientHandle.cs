using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to handle matchmaker-client packets
/// </summary>
public class ClientHandle : MonoBehaviour
{
    public static void HandleInit(Packet _packet)
    {
        // read entry count
        var entryCount = _packet.ReadInt();

        for (int i = 0; i < entryCount; i++)
        {
            // read port
            var port = _packet.ReadInt();
            // read max player count
            var mPlrCount = _packet.ReadInt();

            ServerEntries.Singleton.SetData(new ServerDataEntry(port, mPlrCount));
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
            // read entry index
            var _index = _packet.ReadInt();
            // read player count
            var _playerCount = _packet.ReadInt();
            // read server running state
            var _running = _packet.ReadBool();

            // update server entry
            ServerEntries.Singleton.UpdateData(i, _playerCount, _running);
        }
    }
}