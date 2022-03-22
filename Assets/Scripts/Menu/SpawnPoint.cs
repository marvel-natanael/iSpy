using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Awake() => PlayerSpawner.AddSpawnPoint(transform);
    private void OnDestroy() => PlayerSpawner.RemoveSpawnPoint(transform);
}
