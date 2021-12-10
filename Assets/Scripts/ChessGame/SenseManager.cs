using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseManager : MonoBehaviour
{
    
    private PieceCreator pieceCreator;
    private ChessGameController _chessGameController;
    private Board _board;
    private GameObject[,] senseMatrix = new GameObject[8, 8];
    public GameObject[,] SenseMatrix
    {
        get => senseMatrix;
        set => senseMatrix = value;
    }

    public void SetDependencies(Board board, ChessGameController controller)
    {
        _board = board;
        _chessGameController = controller;
        pieceCreator = _chessGameController.GetComponent<PieceCreator>();
    }

    public void OnSenseSquare(Vector2Int coords)
    {
        _chessGameController.Sense();

        List<Vector2Int> coordinatesToSense = new List<Vector2Int>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector2Int coordsPlusOffset = coords + new Vector2Int(i, j);
                if (_board.CheckIfCoordinatesAreOnBoard(coordsPlusOffset))
                {
                    coordinatesToSense.Add(coordsPlusOffset);
                }
            }
        }

        foreach (var square in coordinatesToSense)
        {
            CheckForOpponentPiece(square);
        }
        
    }

    private void CheckForOpponentPiece(Vector2Int coords)
    {
        Piece pieceOnSquare = _board.GetPieceOnSquare(coords);
        
        if(!pieceOnSquare)
            return;
        
        if (pieceOnSquare.Team != _chessGameController.GetLocalPlayer().Team)
        {
            CreateSensePiece(coords, pieceOnSquare);
        }
    }

    public void CreateSensePiece(Vector2Int coords, Piece pieceToCreate)
    {
        DestroySensePiece(coords);
        GameObject sensePiece = pieceCreator.CreatePiece(pieceToCreate.GetType());
        
        sensePiece.transform.position = _board.CalculatePositionFromCoords(coords);
        sensePiece.GetComponent<PieceAppearenceSetter>().MakeSensePiece();
        senseMatrix[coords.x, coords.y] = sensePiece;
    }

    public void DestroySensePiece(Vector2Int coords)
    {
        if (senseMatrix[coords.x, coords.y])
        {
            Destroy(senseMatrix[coords.x, coords.y]);
        }

        senseMatrix[coords.x, coords.y] = null;
    }

}
