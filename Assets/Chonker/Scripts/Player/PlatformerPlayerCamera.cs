using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlatformerPlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineCamera playerFollowCamera;

    private void Start() {
        _camera.transform.parent = null;
        playerFollowCamera.transform.parent = null;
    }
}
