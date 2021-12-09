using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private bool is2d;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        Initialize();
    }

    public void Initialize()
    {
        originalPosition = transform.position;
        originalRotation = transform.localEulerAngles;
    }

    public void SwitchMode()
    {
        if (GameManager.Instance.is2d)
        {
            GameManager.Instance.is2d = false;
            mainCamera.transform.position = originalPosition;
            mainCamera.transform.rotation = Quaternion.Euler(originalRotation);
            mainCamera.orthographic = false;
        }
        else
        {
            GameManager.Instance.is2d = true;
            mainCamera.transform.position = new Vector3(0, 18, 0);
            mainCamera.transform.rotation = Quaternion.Euler(new Vector3(90, originalRotation.y, 0));
            mainCamera.orthographic = true;
        }
    }
}
