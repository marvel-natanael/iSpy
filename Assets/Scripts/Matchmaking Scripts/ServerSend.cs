using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend : PacketSendInterface
{
    public static void SendInit(ServerDataEntry entry)
    {
        using (Packet _packet = new Packet((int)ServerMatchmakerPackets.initialization))
        {
            _packet.Write(entry.Port);
            _packet.Write(entry.MaxPlayer);

            SendTCPData(_packet);
        }
        Debug.Log($"Sent Init Packet");
    }

    public static void SendUpdate(ServerDataEntry entry)
    {
        using (Packet _packet = new Packet((int)ServerMatchmakerPackets.update))
        {
            _packet.Write(entry.PlayerCount);
            _packet.Write(entry.Running);

            SendTCPData(_packet);
        }
        Debug.Log($"Sent Update Packet");
    }

    public static void SendDisconnect(ServerDataEntry entry)
    {
        using (Packet _packet = new Packet((int)ServerMatchmakerPackets.terminationReply))
        {
            SendTCPData(_packet);
        }
        Debug.Log($"Sent Termination Packet");
    }
}