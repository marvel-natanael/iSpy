using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : PacketSendInterface
{
    public static void SendTag()
    {
        using (Packet _packet = new Packet((int)ClientMatchmakerPackets.tag))
        {
            _packet.Write(false);

            SendTCPData(_packet);
        }
    }

    public static void SendUpdateRequest()
    {
        using (Packet _packet = new Packet((int)ClientMatchmakerPackets.updateRequest))
        {
            SendTCPData(_packet);
        }
    }
}