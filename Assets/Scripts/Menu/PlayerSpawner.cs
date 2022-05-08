using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerPrefab = null, npcs = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private static List<Transform> NPCspawnPoints = new List<Transform>();

    private int nextIndex = 0, npcIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void AddNPCSpawnPoint(Transform transform)
    {
        NPCspawnPoints.Add(transform);

        NPCspawnPoints = NPCspawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);
    public static void RemoveNPCSpawnPoint(Transform transform) => NPCspawnPoints.Remove(transform);

    public override void OnStartServer()
    {
        LobbyNetworkManager.onServerReadied += SpawnPlayer;
        LobbyNetworkManager.onServerReadied += SpawnNPC;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        LobbyNetworkManager.onServerReadied -= SpawnPlayer;
        LobbyNetworkManager.onServerReadied -= SpawnNPC;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab[nextIndex], spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        NetworkServer.Spawn(playerInstance, conn);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);
        nextIndex++;
    }

    [Server]
    public void SpawnNPC(NetworkConnection conn)
    {
        Transform spawnPoint = NPCspawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(npcs[1], NPCspawnPoints[npcIndex].position, NPCspawnPoints[npcIndex].rotation);
        NetworkServer.Spawn(playerInstance);
        //NetworkServer.AddPlayerForConnection(conn, playerInstance);
        npcIndex++;
    }
}