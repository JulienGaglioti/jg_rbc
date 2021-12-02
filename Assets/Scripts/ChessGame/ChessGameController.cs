using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
public abstract class ChessGameController : MonoBehaviour
{
    public enum GameState
    {
        Init, Play, Finished
    }
    
    [SerializeField] private BoardLayout startingBoardLayout;
    private Board _board;
    private UIManager _uiManager;
    private CameraSetup _cameraSetup;
    
    private PieceCreator _pieceCreator;
    protected ChessPlayer _whitePlayer;
    protected ChessPlayer _blackPlayer;
    protected ChessPlayer _activePlayer;
    [SerializeField] protected bool _hasSensed;
    protected GameState _gameState;

    private void Awake()
    {
        _pieceCreator = GetComponent<PieceCreator>();
    }

    protected abstract void SetGameState(GameState state);
    public abstract void TryToStartCurrentGame();
    public abstract bool CanPerformMove();

    public void SetDependencies(UIManager uiManager, Board board, CameraSetup cameraSetup)
    {
        _uiManager = uiManager;
        _board = board;
        _cameraSetup = cameraSetup;
    }

    public bool IsGameInProgress()
    {
        return _gameState == GameState.Play;
    }

    public void CreatePlayers()
    {
        _whitePlayer = new ChessPlayer(TeamColor.White, _board);
        _blackPlayer = new ChessPlayer(TeamColor.Black, _board);
    }

    public void StartNewGame()
    {
        _uiManager.OnChessGameStarted();
        SetGameState(GameState.Init);
        CreatePiecesFromLayout(startingBoardLayout);
        _activePlayer = _whitePlayer;
        GenerateAllPossiblePlayerMoves(_activePlayer);
        TryToStartCurrentGame();
    }

    public void SetupCamera(TeamColor color)
    {
        _cameraSetup.SetupCamera(color);
    }

    public void RestartGame()
    {
        DestroyAllPieces();
        _board.OnGameRestarted();
        _whitePlayer.OnGameRestarted();
        _blackPlayer.OnGameRestarted();
        StartNewGame();
    }

    private void DestroyAllPieces()
    {
        _whitePlayer.ActivePieces.ForEach(p => Destroy(p.gameObject));
        _blackPlayer.ActivePieces.ForEach(p => Destroy(p.gameObject));
    }

    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

            Type type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, team, type);
        }
    }

    private void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type)
    {
        Piece newPiece = _pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, team, _board);

        Material teamMaterial = _pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);

        _board.SetPieceOnBoard(squareCoords, newPiece);

        ChessPlayer currentPlayer = team == TeamColor.White ? _whitePlayer : _blackPlayer;
        currentPlayer.AddPiece(newPiece);
    }

    public void EndTurn()
    {
        GenerateAllPossiblePlayerMoves(_activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(_activePlayer));
        if (CheckIfGameIsFinished())
        {
            EndGame();
        }
        else
        {
            _hasSensed = false;
            ChangeActiveTeam();
        }
    }

    private bool CheckIfGameIsFinished()
    {
        // Basically, returns true if it's Check Mate
        Piece[] piecesAttackingKing = _activePlayer.GetPiecesAttackingPieceOfType<King>();
        if (piecesAttackingKing.Length > 0)
        {
            ChessPlayer opponent = GetOpponentToPlayer(_activePlayer);
            Piece attackedKing = opponent.GetPiecesOfType<King>().FirstOrDefault();
            opponent.RemoveMovesEnablingAttackOnPieceOfType<King>(_activePlayer, attackedKing);

            int availableKingMoves = attackedKing.availableMoves.Count;
            if (availableKingMoves == 0)
            {
                bool canCoverKing = opponent.CanHidePieceFromAttack<King>(_activePlayer);
                if (!canCoverKing)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void EndGame()
    {
        _uiManager.OnChessGameFinished(_activePlayer.Team.ToString());
        SetGameState(GameState.Finished);
    }

    private void GenerateAllPossiblePlayerMoves(ChessPlayer player)
    {
        player.GenerateAllPossibleMoves();
    }

    public bool IsTeamTurnActive(TeamColor team)
    {
        return (_activePlayer.Team == team);
    }

    public bool HasPlayerSensed()
    {
        return _hasSensed;
    }

    public void Sense()
    {
        // Debug.LogError("sensed");
        _hasSensed = true;
    }

    private void ChangeActiveTeam()
    {
        _activePlayer = _activePlayer == _whitePlayer ? _blackPlayer : _whitePlayer;
    }

    private ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == _whitePlayer ? _blackPlayer : _whitePlayer;
    }

    internal void RemoveMovesEnablingAttackOnPieceOfType<T>(Piece piece) where T : Piece
    {
        _activePlayer.RemoveMovesEnablingAttackOnPieceOfType<T>(GetOpponentToPlayer(_activePlayer), piece);
    }

    public void OnPieceRemoved(Piece piece)
    {
        ChessPlayer pieceOwner = (piece.Team == TeamColor.White) ? _whitePlayer : _blackPlayer;
        pieceOwner.RemovePiece(piece);
    }
}
