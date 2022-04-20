using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketSendInterface : MonoBehaviour
{
    protected static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        MatchmakerClient.instance.transport.SendData(_packet);
    }
}