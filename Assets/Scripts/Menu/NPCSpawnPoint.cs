using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnPoint : MonoBehaviour
{
    private void Awake() => PlayerSpawner.AddNPCSpawnPoint(transform);
    private void OnDestroy() => PlayerSpawner.RemoveNPCSpawnPoint(transform);
}
