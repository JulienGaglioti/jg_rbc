using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    protected IInputHandler[] inputHandlers;
    protected GameObject hitGameObject;

    public abstract void OnInputReceived();

    private void Awake()
    {
        inputHandlers = GetComponents<IInputHandler>();
    }
}
