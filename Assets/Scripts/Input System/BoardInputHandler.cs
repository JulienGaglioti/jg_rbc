using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour, IInputHandler
{
    private Board _board;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action callback, bool buttonDown)
    {
        if (buttonDown)
        {
            _board.OnButtonDown(inputPosition);
        }
        else
        {
            _board.OnButtonUp(inputPosition);
        }
    }
}
