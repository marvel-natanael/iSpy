using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] npcs = null;

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
        RoomNetManager.onServerReadied += SpawnNPC;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        RoomNetManager.onServerReadied -= SpawnNPC;
    }

    [Server]
    public void SpawnNPC(NetworkConnection conn)
    {
        if (!NetworkServer.active) { return; }

        Transform spawnPoint = NPCspawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        for (int i = 0; i < NPCspawnPoints.Count; i++)
        {
            GameObject npcInstance = Instantiate(npcs[0], NPCspawnPoints[i].position, NPCspawnPoints[i].rotation);
            NetworkServer.Spawn(npcInstance);
        }
    }
}