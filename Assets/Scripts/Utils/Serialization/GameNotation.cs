using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[System.Serializable]
public class GameNotation
{
    public Moves requested_moves;
    public Moves taken_moves;
        
    public static GameNotation CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameNotation>(jsonString);
    }

    [System.Serializable]
    public class Moves
    {
        public MoveInfo[] white;
        public MoveInfo[] black;
    }

    [System.Serializable]
    public class MoveInfo
    {
        public string type;
        public string value;
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}