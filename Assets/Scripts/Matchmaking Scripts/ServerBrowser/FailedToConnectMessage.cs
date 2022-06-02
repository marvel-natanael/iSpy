using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedToConnectMessage : MonoBehaviour
{
    //[SerializeField] private string animName;

    private void Start()
    {
        //GetComponent<Animator>().Play(animName);
        Invoke(nameof(Vanish), 1f);
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}