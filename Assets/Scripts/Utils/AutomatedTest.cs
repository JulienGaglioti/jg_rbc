using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class AutomatedTest : MonoBehaviour
{
    public int turnCounter;
    public bool whiteTurn = true;
    [SerializeField] private JSONReader jsonReader;
    public GameNotation.Moves requestedMoves;
    public GameNotation.Moves takenMoves;

    private Board _board;

    private void Start()
    {
        jsonReader.ReadFromJSON();
        requestedMoves = jsonReader.gameNotation.requested_moves;
        takenMoves = jsonReader.gameNotation.taken_moves;
    }

    public void SetDependencies(Board board)
    {
        _board = board;
    }

    public void NextTurn()
    {
        if (whiteTurn)
        {
            if (turnCounter == 13)
            {
                Debug.Log(requestedMoves.white[turnCounter].value);
            }
            string pieceSelection = requestedMoves.white[turnCounter].value[0].ToString() + 
                                    requestedMoves.white[turnCounter].value[1].ToString();
            _board.SetSelectedPiece(VectorConverter.ConvertToVector(pieceSelection));

            string pieceMovement = requestedMoves.white[turnCounter].value[2].ToString() +  
                                   requestedMoves.white[turnCounter].value[3].ToString();
            _board.AttemptMovement(VectorConverter.ConvertToVector(pieceMovement));

            whiteTurn = false;
        }
        else
        {
            string pieceSelection = requestedMoves.black[turnCounter].value[0].ToString() +
                                    requestedMoves.black[turnCounter].value[1].ToString();
            _board.SetSelectedPiece(VectorConverter.ConvertToVector(pieceSelection));

            string pieceMovement = requestedMoves.black[turnCounter].value[2].ToString() +
                                   requestedMoves.black[turnCounter].value[3].ToString();
            _board.AttemptMovement(VectorConverter.ConvertToVector(pieceMovement));
            
            Debug.Log(turnCounter);
            whiteTurn = true;
            turnCounter++;
        }
    }
}
