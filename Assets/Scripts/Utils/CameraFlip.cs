using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlip : MonoBehaviour
{
    private Camera mainCamera;
    public EmptyEventChannelSO cameraFlipped;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void SetupCamera(TeamColor team)
    {
        if (team == TeamColor.Black)
        {
            FlipCamera();
        }
    }

    public void FlipCamera()
    {
        GameManager.Instance.cameraFlipped = !GameManager.Instance.cameraFlipped;
        cameraFlipped.RaiseEvent();
        
        mainCamera.transform.position = 
            new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -mainCamera.transform.position.z);
        mainCamera.transform.Rotate(Vector3.up, 180f, Space.World);
    }
}
