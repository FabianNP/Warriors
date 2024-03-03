using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToFollow : MonoBehaviour
{
    private Transform objectToFollow;
    private CinemachineStateDrivenCamera virtualCamera;
    private CinemachineVirtualCamera vcamA;

    void Start()
    {
        virtualCamera = GetComponent<Cinemachine.CinemachineStateDrivenCamera>();

        if (virtualCamera != null)
        {
            vcamA = virtualCamera.ChildCameras[0].VirtualCameraGameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }
        else
        {
            Debug.LogError("No se encontró el componente CinemachineStateDrivenCamera en este objeto.");
        }

    }

    public void SetObjectTransform(Transform transform)
    {
        objectToFollow = transform;

        if (vcamA != null)
        {
            vcamA.Follow = objectToFollow.transform;
        }
        else
        {
            Debug.LogError("No se encontró el componente CinemachineVirtualCamera en el primer ChildCameras de la State-Driven Camera.");
        }

    }
}
