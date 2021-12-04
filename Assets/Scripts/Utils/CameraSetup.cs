using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    private Camera mainCamera;

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
        GetComponent<CameraSwitch>().Initialize();
        
         // after I flip the camera and every time I create a piece, its Y rotation must be set to the Z rotation of the main camera
    }

    private void FlipCamera()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y,
            -mainCamera.transform.position.z);
        mainCamera.transform.Rotate(Vector3.up, 180f, Space.World);
    }
}
