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

    private int turIndex, verIndex, horIndex;

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
        RoomNetManager.OnServerReadied += SpawnNPC;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        RoomNetManager.OnServerReadied -= SpawnNPC;
    }

    [Server]
    public void SpawnNPC(NetworkConnection conn)
    {
        if (!NetworkServer.active) { return; }
        //soldier vertical

        for (int i = turIndex; i < TurretSpawnPoints.Count; i++)
            SpawnTurret();
        for (int i = verIndex; i < VerSoldierSpawnPoints.Count; i++)
            SpawnVer();
        for (int i = horIndex; i < HorSoldierSpawnPoints.Count; i++)
            SpawnHor();

        /*
        for (int i = verIndex; i < VerSoldierSpawnPoints.Count; i++)
            Spawn(VerSoldierSpawnPoints, npcs[1],verIndex);
        for (int i = horIndex; i < HorSoldierSpawnPoints.Count; i++)
            Spawn(HorSoldierSpawnPoints, npcs[2], horIndex);*/
        //soldier horizontal
        //Spawn(NPCspawnPoints, npcs[2]);
        //turret
        //Spawn(TurretSpawnPoints, npcs[1]);
    }

    public virtual void SpawnTurret()
    {
        if (!NetworkServer.active) { return; }

        Transform spawnPoint = TurretSpawnPoints.ElementAtOrDefault(turIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {turIndex}");
            return;
        }

        //GameObject npcInstance = Instantiate(npcToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
        GameObject npcInstance = Instantiate(npcs[0], TurretSpawnPoints[turIndex].position, npcs[0].transform.rotation);
        NetworkServer.Spawn(npcInstance);
        turIndex++;
    }

    public virtual void SpawnVer()
    {
        if (!NetworkServer.active) { return; }

        Transform spawnPoint = VerSoldierSpawnPoints.ElementAtOrDefault(verIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {verIndex}");
            return;
        }

        //GameObject npcInstance = Instantiate(npcToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
        GameObject npcInstance = Instantiate(npcs[1], VerSoldierSpawnPoints[verIndex].position, npcs[1].transform.rotation);
        NetworkServer.Spawn(npcInstance);
        verIndex++;
    }

    public virtual void SpawnHor()
    {
        if (!NetworkServer.active) { return; }

        Transform spawnPoint = HorSoldierSpawnPoints.ElementAtOrDefault(horIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {horIndex}");
            return;
        }

        //GameObject npcInstance = Instantiate(npcToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
        GameObject npcInstance = Instantiate(npcs[2], HorSoldierSpawnPoints[horIndex].position, npcs[2].transform.rotation);
        NetworkServer.Spawn(npcInstance);
        horIndex++;
    }
}