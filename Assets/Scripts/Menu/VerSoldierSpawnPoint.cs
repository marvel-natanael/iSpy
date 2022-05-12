using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerSoldierSpawnPoint : SpawnPoint
{
    public override void AddToList()
    {
        PlayerSpawner.AddVerSoldierSpawnPoint(transform);
    }
    public override void RemoveFromList()
    {
        PlayerSpawner.RemoveVerSoldierSpawnPoint(transform);
    }
}
