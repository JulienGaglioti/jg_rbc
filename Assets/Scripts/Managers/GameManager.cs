using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool is2d = true;
    public bool makePiecesInvisible;
    public bool cameraFlipped;

    public void SetInvisibleMode(bool b)
    {
        makePiecesInvisible = b;
    }
}
