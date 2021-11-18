using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    private enum GameState
    {
        Init, Play, Finished
    }
    
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;
    
    private PieceCreator _pieceCreator;
    private ChessPlayer _whitePlayer;
    private ChessPlayer _blackPlayer;
    private ChessPlayer _activePlayer;
    private GameState _gameState;

    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
    }

    private void SetDependencies()
    {
        _pieceCreator = GetComponent<PieceCreator>();
    }
    
    private void Start()
    {
        StartNewGame();
    }

    private void SetGameState(GameState state)
    {
        _gameState = state;
    }

    public bool IsGameInProgress()
    {
        return _gameState == GameState.Play;
    }

    private void CreatePlayers()
    {
        _whitePlayer = new ChessPlayer(TeamColor.White, board);
        _blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }

    private void StartNewGame()
    {
        SetGameState(GameState.Init);
        uiManager.HideEndGameScreen();
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        _activePlayer = _whitePlayer;
        GenerateAllPossiblePlayerMoves(_activePlayer);
        SetGameState(GameState.Play);
    }

    public void RestartGame()
    {
        DestroyAllPieces();
        board.OnGameRestarted();
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
        newPiece.SetData(squareCoords, team, board);

        Material teamMaterial = _pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

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
            ChangeActiveTeam();
        }
    }

    private bool CheckIfGameIsFinished()
    {
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
        SetGameState(GameState.Finished);
        uiManager.OnGameFinished(_activePlayer.Team.ToString());
    }

    private void GenerateAllPossiblePlayerMoves(ChessPlayer player)
    {
        player.GenerateAllPossibleMoves();
    }

    public bool IsTeamTurnActive(TeamColor team)
    {
        return (_activePlayer.Team == team);
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
