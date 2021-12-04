using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceModeSwitch : MonoBehaviour
{
    public GameObject gameObject3d;
    public GameObject gameObject2d;
    
    private bool _is2d;

    public void SwitchMode()
    {
        if (_is2d)
        {
            _is2d = false;
            gameObject2d.SetActive(false);
            gameObject3d.SetActive(true);
        }
        else
        {
            _is2d = true;
            gameObject2d.SetActive(true);
            gameObject3d.SetActive(false);
        }
    }
}
