using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensePlatformInputHandler : MonoBehaviour, IInputHandler
{
    public Piece piece;
    private Board _board;

    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action callback, bool buttonDown)
    {
        if (buttonDown && _board != null)
        {
            _board.SelectSensePieceFromPool(piece);
        }
    }

    public void SetBoard(Board board)
    {
        _board = board;
    }
}
