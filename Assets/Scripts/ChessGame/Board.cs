using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece _selectedPiece;
    private ChessGameController _chessController;
    private SquareSelectorCreator _squareSelectorCreator;

    private void Awake()
    {
        _squareSelectorCreator = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(ChessGameController chessController)
    {
        _chessController = chessController;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        if(!_chessController.IsGameInProgress())
            return;
        
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece clickedPiece = GetPieceOnSquare(coords);
        
        // Debug.Log("coord: "+coords+", piece: "+clickedPiece);
        
        if (_selectedPiece)
        {
            if (clickedPiece != null && _selectedPiece == clickedPiece)
                DeselectPiece();
            else if (clickedPiece != null && _selectedPiece != clickedPiece && _chessController.IsTeamTurnActive(clickedPiece.Team))
                SelectPiece(clickedPiece);
            else if (_selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords);
        }
        else
        {
            if (clickedPiece != null && _chessController.IsTeamTurnActive(clickedPiece.Team))
            {
                SelectPiece(clickedPiece);
            }
        }
    }

    private void SelectPiece(Piece piece)
    {
        _chessController.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        _selectedPiece = piece;
        List<Vector2Int> indicatorSquares = _selectedPiece.availableMoves;
        ShowIndicatorSquares(indicatorSquares);
    }

    private void ShowIndicatorSquares(List<Vector2Int> indicatorSquares)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < indicatorSquares.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(indicatorSquares[i]);
            bool isSquareFree = GetPieceOnSquare(indicatorSquares[i]) == null;
            squaresData.Add(position, isSquareFree);
        }

        _squareSelectorCreator.ShowSelection(squaresData);
    }

    private void DeselectPiece()
    {
        _selectedPiece = null;
        _squareSelectorCreator.ClearSelection();
    }

    internal void OnSelectedPieceMoved(Vector2Int intCoords)
    {
        // Debug.Log("Moving " + _selectedPiece.name + " on " + intCoords);
        TryToTakeOpponentPiece(intCoords);
        UpdateBoardOnPieceMove(intCoords, _selectedPiece.OccupiedSquare, _selectedPiece, null);
        _selectedPiece.MovePiece(intCoords);
        DeselectPiece();
        EndTurn();
    }

    private void TryToTakeOpponentPiece(Vector2Int intCoords)
    {
        Piece piece = GetPieceOnSquare(intCoords);
        if (piece && !_selectedPiece.IsFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }

    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.OccupiedSquare.x, piece.OccupiedSquare.y] = null;
            _chessController.OnPieceRemoved(piece);
            Destroy(piece.gameObject);
        }
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }
    

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }

    private void EndTurn()
    {
        _chessController.EndTurn();
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }

        return false;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }
}
