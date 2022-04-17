using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        MatchmakerClient.instance.transport.SendData(_packet);
    }

    #region SendFunctions

    public static void SendInit()
    {
        using (Packet _packet = new Packet((int)ClientPackets.init))
        {
            SendTCPData(_packet);
        }
    }

    public static void SendUpdate(ServerDataEntry entry)
    {
        using (Packet _packet = new Packet((int)ClientPackets.update))
        {
            _packet.Write(entry.Port);
            _packet.Write(entry.PlayerCount);
            _packet.Write(entry.Running);

            SendTCPData(_packet);
        }
    }

    public static void SendDisconnect()
    {
        using (Packet _packet = new Packet((int)ClientPackets.disconnect))
        {
            SendTCPData(_packet);
        }
    }

    #endregion SendFunctions
}