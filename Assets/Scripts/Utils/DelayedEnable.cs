using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEnable : MonoBehaviour
{
    public MonoBehaviour monoBehaviour;

    private void OnEnable()
    {
        Invoke("DelayEnable", 1);
    }

    private void DelayEnable()
    {
        monoBehaviour.enabled = true;
    }
}
