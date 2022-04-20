using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : PacketSendInterface
{
    public static void SendUpdateRequest()
    {
        using (Packet _packet = new Packet((int)ClientMatchmakerPackets.updateRequest))
        {
            _packet.Write(MatchmakerClient.instance.ID);
            SendTCPData(_packet);
        }
    }
}