using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnPoint : SpawnPoint
{
    public override void AddToList()
    {
        PlayerSpawner.AddTurretSpawnPoint(transform);
    }
    public override void RemoveFromList()
    {
        PlayerSpawner.RemoveTurretSpawnPoint(transform);
    }
}
