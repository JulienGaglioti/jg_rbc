using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorConverter
{
    private static Dictionary<char, int> charToIntDic = new Dictionary<char, int > {
        {
            'a', 0
        },
        {
            'b', 1
        },
        {
            'c', 2
        },
        {
            'd', 3
        },
        {
            'e', 4
        },
        {
            'f', 5
        },
        {
            'g', 6
        },
        {
            'h', 7
        },
        {
            '1', 0
        },
        {
            '2', 1
        },
        {
            '3', 2
        },
        {
            '4', 3
        },
        {
            '5', 4
        },
        {
            '6', 5
        },
        {
            '7', 6
        },
        {
            '8', 7
        },
    };
    
    public static Vector2Int ConvertToVector(string s)
    {
        charToIntDic.TryGetValue(s[0], out int x);
        charToIntDic.TryGetValue(s[1], out int y);
        // Debug.Log(x + ", " + y);
        return new Vector2Int(x, y);
    }
}
