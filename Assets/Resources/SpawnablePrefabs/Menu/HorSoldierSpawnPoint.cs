using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorSoldierSpawnPoint : SpawnPoint
{
    public override void AddToList()
    {
        PlayerSpawner.AddHorSoldierSpawnPoint(transform);
    }
    public override void RemoveFromList()
    {
        PlayerSpawner.RemoveHorSoldierSpawnPoint(transform);
    }
}
