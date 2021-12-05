using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseManager : MonoBehaviour
{
    
    private PieceCreator pieceCreator;
    private ChessGameController _chessGameController;
    private Board _board;
    private Piece[,] senseMatrix = new Piece[8, 8];

    public void SetDependencies(Board board, ChessGameController controller)
    {
        _board = board;
        _chessGameController = controller;
        pieceCreator = _chessGameController.GetComponent<PieceCreator>();
    }

    public void SenseSquare(Vector2Int coords)
    {
        _chessGameController.Sense();
        pieceCreator.CreatePiece(typeof(King)).GetComponent<TeamColorSetter>().MakeSensePiece();
    }

}
