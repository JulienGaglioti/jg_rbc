using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButtonDependency : MonoBehaviour
{
    private ChessGameController _controller;

    public void SetController(ChessGameController controller)
    {
        _controller = controller;
    }

    public void Pass()
    {
        _controller.PassTurn();
    }
}
