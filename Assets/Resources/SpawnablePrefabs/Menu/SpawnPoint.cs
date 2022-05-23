using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public virtual void AddToList()
    {
    }

    public virtual void RemoveFromList()
    {
    }
    private void Awake() => AddToList();
    private void OnDestroy() => RemoveFromList();
}
