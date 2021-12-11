using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[System.Serializable]
public class GameNotation
{
    public string type;
    private string win_reason;
    public string winner_color;
    public string black_name;
    public string white_name;
    private string capture_squares;
    private string fens_after_move;
    private string fens_before_move;
    public Moves requested_moves;
    private string sense_results;
    private string senses;
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

}