using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to handle matchmaker-server packets
/// </summary>
public class ServerHandle : MonoBehaviour
{
    public static void HandleUpdateReq(Packet _packet)
    {
        // todo, Networking-Matchmaking: Handle update request from matchmaker.
    }

    public static void HandleTerminationReq(Packet _packet)
    {
        // todo, Networking-Matchmaking: Handle termination request from matchmaker.
    }
}