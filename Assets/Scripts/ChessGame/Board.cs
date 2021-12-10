using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public abstract class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece _selectedPiece;
    public GameObject _selectedSensePiece;
    private bool sensePieceFromPool;
    private Vector2Int _selectedSenseCoords;
    private Vector2Int _selectedSenseSquare;
    private ChessGameController _chessController;
    private SenseManager _senseManager;
    private SquareSelectorCreator _squareSelectorCreator;

    protected virtual void Awake()
    {
        _squareSelectorCreator = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void OnButtonDown(Vector3 inputPosition)
    {
        if (!_chessController)
            return;
        
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece clickedPiece = GetPieceOnSquare(coords);
        
        // Just using enums and nested if statements for now. Change to state pattern later if there's some time left
        if (_chessController.turnState == ChessGameController.TurnState.Sense)
        {
            HandleSenseClick(coords);
        }
        else if (_chessController.turnState == ChessGameController.TurnState.Move)
        {
            if (clickedPiece != null)
            {
                if (_chessController.IsTeamTurnActive(clickedPiece.Team))
                {
                    SelectPiece(coords);
                }
            }
            if (CheckIfCoordinatesAreOnBoard(coords) && _senseManager.SenseMatrix[coords.x, coords.y] != null)
            {
                SelectSensePiece(coords);
            }
        }
        else if (_chessController.turnState == ChessGameController.TurnState.Wait)
        {
            if (CheckIfCoordinatesAreOnBoard(coords) && _senseManager.SenseMatrix[coords.x, coords.y] != null)
            {
                SelectSensePiece(coords);
            }
        }
    }

    public void OnButtonUp(Vector3 inputPosition)
    {
        if (!_chessController)
            return;

        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        
        if (_chessController.turnState == ChessGameController.TurnState.Move)
        {
            if (_selectedPiece)
            {
                if (_selectedPiece.CanMoveTo(coords))
                {
                    AttemptMovement(coords);
                }
                else
                {
                    DeselectPiece();
                }
            }
            else if (_selectedSensePiece)
            {
                if (CheckIfCoordinatesAreOnBoard(coords))
                {
                    _senseManager.CreateSensePiece(coords, _selectedSensePiece.GetComponent<Piece>());
                    if (!sensePieceFromPool)
                    {
                        _senseManager.DestroySensePiece(_selectedSenseCoords);
                    }
                    DeselectSensePiece();
                }
                else
                {
                    if (!sensePieceFromPool)
                    {
                        _senseManager.DestroySensePiece(_selectedSenseCoords);
                    }
                    DeselectSensePiece();
                }
            }
        }
        else if (_chessController.turnState == ChessGameController.TurnState.Wait)
        {
            if (_selectedSensePiece)
            {
                if (CheckIfCoordinatesAreOnBoard(coords))
                {
                    _senseManager.CreateSensePiece(coords, _selectedSensePiece.GetComponent<Piece>());
                    if (!sensePieceFromPool)
                    {
                        _senseManager.DestroySensePiece(_selectedSenseCoords);
                    }

                    DeselectSensePiece();
                }
                else
                {
                    if (!sensePieceFromPool)
                    {
                        _senseManager.DestroySensePiece(_selectedSenseCoords);
                    }
                    DeselectSensePiece();
                }
            }
        }
    }

    private void AttemptMovement(Vector2Int coords)
    {
        Vector2Int realCoords = _selectedPiece.AttemptMove(coords);
        SelectedPieceMoved(realCoords);
    }

    public abstract void SelectedPieceMoved(Vector2 coords);
    public abstract void SetSelectedPiece(Vector2 coords);

    public void SetDependencies(ChessGameController chessController, SenseManager senseManager)
    {
        _chessController = chessController;
        _senseManager = senseManager;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    

    public void OnSquareSelected(Vector3 inputPosition)
    {
        if(!_chessController || !_chessController.CanPerformMove())
            return;
        
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece clickedPiece = GetPieceOnSquare(coords);
        
        // Debug.Log("coord: "+coords+", piece: "+clickedPiece);

        HandlePieceSelection(coords, clickedPiece);
    }

    private void HandlePieceSelection(Vector2Int coords, Piece clickedPiece)
    {
        if (_selectedPiece)
        {
            if (clickedPiece != null && _selectedPiece == clickedPiece)
                DeselectPiece();
            else if (clickedPiece != null && _selectedPiece != clickedPiece &&
                     _chessController.IsTeamTurnActive(clickedPiece.Team))
                SelectPiece(coords);
            else if (_selectedPiece.CanMoveTo(coords))
                SelectedPieceMoved(coords);
        }
        else
        {
            if (clickedPiece != null && _chessController.IsTeamTurnActive(clickedPiece.Team))
            {
                SelectPiece(coords);
            }
        }
    }

    private void HandleSenseClick(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
        {
            _senseManager.OnSenseSquare(coords);
        }
    }

    private void ShowSenseIndicator()
    {
        _squareSelectorCreator.ClearSelection();
        
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 position = CalculatePositionFromCoords(_selectedSenseSquare+new Vector2Int(i, j));
                squaresData.Add(position, true);
            }
        }
        
        _squareSelectorCreator.ShowSelection(squaresData);
    }

    private void SelectSensePiece(Vector2Int coords)
    {
        _selectedSenseCoords = coords;
        _selectedSensePiece = _senseManager.SenseMatrix[coords.x, coords.y];
    }

    public void SelectSensePieceFromPool(Piece piece)
    {
        sensePieceFromPool = true;
        _selectedSensePiece = piece.gameObject;
    }

    private void DeselectSensePiece()
    {
        _selectedSenseCoords = Vector2Int.zero;
        _selectedSensePiece = null;
        sensePieceFromPool = false;
    }

    private void SelectPiece(Vector2Int coords)
    {
        // Piece piece = GetPieceOnSquare(coords);
        // _chessController.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        SetSelectedPiece(coords);
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

    public void OnSelectedPieceMoved(Vector2Int intCoords)
    {
        // Debug.Log("Moving " + _selectedPiece.name + " on " + intCoords);
        _senseManager.DestroySensePiece(intCoords);
        TryToTakeOpponentPiece(intCoords);
        UpdateBoardOnPieceMove(intCoords, _selectedPiece.OccupiedSquare, _selectedPiece, null);
        _selectedPiece.MovePiece(intCoords);
        DeselectPiece();
        EndTurn();
    }

    public void OnSetSelectedPiece(Vector2Int intCoords)
    {
        Piece piece = GetPieceOnSquare(intCoords);
        _selectedPiece = piece;
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
        {
            return grid[coords.x, coords.y];
        }
        return null;
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
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

    public void OnGameRestarted()
    {
        _selectedPiece = null;
        CreateGrid();
    }
}
