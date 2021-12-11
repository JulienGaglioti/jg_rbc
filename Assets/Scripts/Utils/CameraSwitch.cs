using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera mainCamera;
    public Vector3 ortographicPosition;
    public Vector3 defaultPerspectivePosition;
    public Vector3 flippedPerspectivePosition;
    public EmptyEventChannelSO cameraModeSwitched;
    private bool is2d;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void SwitchMode()
    {
        GameManager.Instance.is2d = !GameManager.Instance.is2d;
        mainCamera.orthographic = GameManager.Instance.is2d;
        cameraModeSwitched.RaiseEvent();
        
        if (GameManager.Instance.is2d)
        {
            mainCamera.transform.position = ortographicPosition;
            if (!GameManager.Instance.cameraFlipped)
            {
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
            else
            {
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, -180));
            }
        }
        else
        {
            if (!GameManager.Instance.cameraFlipped)
            {
                mainCamera.transform.position = defaultPerspectivePosition;
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            }
            else
            {
                mainCamera.transform.position = flippedPerspectivePosition;
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(45, 180, 0));
            }
        }
    }
}
