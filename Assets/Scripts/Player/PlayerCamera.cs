using Mirror;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

    public override void OnStartAuthority()
    {
        virtualCamera.gameObject.SetActive(true);
    }
}