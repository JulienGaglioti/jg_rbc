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

    public enum TurnState
    {
        Sense, Move, Wait
    }
    
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private StringEventChannelSO infoBox;
    
    private Board _board;
    private UIManager _uiManager;
    private CameraSetup _cameraSetup;
    
    private PieceCreator _pieceCreator;
    protected ChessPlayer _whitePlayer;
    protected ChessPlayer _blackPlayer;
    protected ChessPlayer _activePlayer;
    protected ChessPlayer _localPlayer;
    [SerializeField] protected bool _hasSensed;
    protected GameState _gameState;
    public TurnState turnState;

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

    public void SetTurnState(TurnState state)
    {
        turnState = state;
        infoBox.RaiseEvent(state + " state");
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

        newPiece.SetTeamColor(team);

        _board.SetPieceOnBoard(squareCoords, newPiece);

        ChessPlayer currentPlayer = team == TeamColor.White ? _whitePlayer : _blackPlayer;
        currentPlayer.AddPiece(newPiece);
    }

    public void EndTurn()
    {
        GenerateAllPossiblePlayerMoves(_activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(_activePlayer));
        if (IsKingDead())
        {
            EndGame();
        }
        else
        {
            _hasSensed = false;
            ChangeActiveTeam();
        }
    }

    private bool IsKingDead()
    {
        ChessPlayer opponent = GetOpponentToPlayer(_activePlayer);
        if (opponent.GetPiecesOfType<King>().Length == 0)
        {
            print($"{_activePlayer} won");
            return true;
        }
        
        return false;

    }

    private bool IsCheckMate()
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
        SetTurnState(TurnState.Move);
        _hasSensed = true;
    }

    private void ChangeActiveTeam()
    {
        _activePlayer = _activePlayer == _whitePlayer ? _blackPlayer : _whitePlayer;
        if (turnState == TurnState.Move)
        {
            SetTurnState(TurnState.Wait);
        }
        else
        {
            SetTurnState(TurnState.Sense);
        }
    }

    protected ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == _whitePlayer ? _blackPlayer : _whitePlayer;
    }

    public ChessPlayer GetLocalPlayer()
    {
        return _localPlayer;
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
