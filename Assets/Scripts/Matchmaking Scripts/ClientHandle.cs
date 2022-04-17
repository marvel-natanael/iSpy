using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void HandleInit(Packet _packet)
    {
        // todo, Networking-Matchmaking: Handle initialization request from matchmaker.
    }

    public static void HandleUpdateReq(Packet _packet)
    {
        // todo, Networking-Matchmaking: Handle update request from matchmaker.
    }

    public static void HandleTerminationReq(Packet _packet)
    {
        // todo, Networking-Matchmaking: Handle termination request from matchmaker.
    }
}