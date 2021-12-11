using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset json;
    public GameNotation gameNotation;

    [ContextMenu("Create")]
    public void ReadFromJSON()
    {
        gameNotation = GameNotation.CreateFromJSON(json.text);
    }
}
