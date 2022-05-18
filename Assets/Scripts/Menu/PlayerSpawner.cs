using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] npcs = null;

    private static List<Transform> VerSoldierSpawnPoints = new List<Transform>();
    private static List<Transform> HorSoldierSpawnPoints = new List<Transform>();
    private static List<Transform> TurretSpawnPoints = new List<Transform>();

    private int nextIndex = 0, npcIndex = 0;

    public static void AddVerSoldierSpawnPoint(Transform transform)
    {
        AddSpawnPoint(transform, VerSoldierSpawnPoints);
    }
    public static void AddTurretSpawnPoint(Transform transform)
    {
        AddSpawnPoint(transform, TurretSpawnPoints);
    }
    public static void AddHorSoldierSpawnPoint(Transform transform)
    {
        AddSpawnPoint(transform, HorSoldierSpawnPoints);
    }
    public static void RemoveVerSoldierSpawnPoint(Transform transform) => VerSoldierSpawnPoints.Remove(transform);
    public static void RemoveHorSoldierSpawnPoint(Transform transform) => HorSoldierSpawnPoints.Remove(transform);
    public static void RemoveTurretSpawnPoint(Transform transform) => TurretSpawnPoints.Remove(transform);

    public static void AddSpawnPoint(Transform transform, List<Transform> list)
    {
        list.Add(transform);
        list = list.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform, List<Transform> list)
    {
        list.Remove(transform);
    }

    public override void OnStartServer()
    {
        RoomNetManager.onServerReadied += SoawnNPC;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        RoomNetManager.onServerReadied -= SoawnNPC;
    }

    [Server]
    public void SoawnNPC(NetworkConnection conn)
    {
        //soldier vertical
        Spawn(VerSoldierSpawnPoints, npcs[0]);        
        //soldier horizontal
        //Spawn(NPCspawnPoints, npcs[2]);
        //turret
        Spawn(TurretSpawnPoints, npcs[1]);

    }

    public virtual void Spawn(List<Transform> spawnPoints, GameObject npcToSpawn)
    {
        if (!NetworkServer.active) { return; }

        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            //GameObject npcInstance = Instantiate(npcToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
            GameObject npcInstance = Instantiate(npcToSpawn, spawnPoints[i].position, npcToSpawn.transform.rotation);
            NetworkServer.Spawn(npcInstance);
        }
    }
}