using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessPlayer
{
    public TeamColor Team { get; set; }
    public Board ChessBoard { get; set; }
    public List<Piece> ActivePieces { get; private set; }
    public ChessPlayer(TeamColor team, Board chessBoard)
    {
        ActivePieces = new List<Piece>();
        ChessBoard = chessBoard;
        Team = team;
    }

    public void AddPiece(Piece piece)
    {
        if (!ActivePieces.Contains(piece))
            ActivePieces.Add(piece);
    }

    public void RemovePiece(Piece piece)
    {
        if (ActivePieces.Contains(piece))
            ActivePieces.Remove(piece);
    }

    

    public void GenerateAllPossibleMoves()
    {
        foreach (var piece in ActivePieces)
        {
            if (ChessBoard.HasPiece(piece))
            {
                piece.SelectAvailableSquares();
            }
        }
    }

    public Piece[] GetPiecesOfType<T>() where T : Piece
    {
        return ActivePieces.Where(p => p is T).ToArray();
    }

    // returns the array of pieces attacking the piece of type T
    public Piece[] GetPiecesAttackingPieceOfType<T>() where  T : Piece
    {
        return ActivePieces.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
    }

    // given a selected piece, remove moves that would enable an attack of my piece of type T (i.e King)
    public void RemoveMovesEnablingAttackOnPieceOfType<T>(ChessPlayer opponent, Piece selectedPiece) where T : Piece
    {
        List<Vector2Int> coordsToRemove = new List<Vector2Int>();
        foreach (var square in selectedPiece.availableMoves)
        {
            Piece pieceOnSquare = ChessBoard.GetPieceOnSquare(square); // temp piece to use after simulating the move
            ChessBoard.UpdateBoardOnPieceMove(square, selectedPiece.OccupiedSquare, selectedPiece, null);
            opponent.GenerateAllPossibleMoves();
            if (opponent.CheckIfIsAttackingPieceOfType<T>())
            {
                coordsToRemove.Add(square);
            }

            ChessBoard.UpdateBoardOnPieceMove(selectedPiece.OccupiedSquare, square, selectedPiece, pieceOnSquare);
        }

        foreach (var coords in coordsToRemove)
        {
            selectedPiece.availableMoves.Remove(coords);
        }
    }

    private bool CheckIfIsAttackingPieceOfType<T>() where T : Piece
    {
        foreach (var piece in ActivePieces)
        {
            if (ChessBoard.HasPiece(piece) && piece.IsAttackingPieceOfType<T>())
            {
                return true;
            }
        }

        return false;
    }

    public bool CanHidePieceFromAttack<T>(ChessPlayer opponent) where T : Piece
    {
        foreach (var piece in ActivePieces)
        {
            foreach (var square in piece.availableMoves)
            {
                Piece pieceOnSquare = ChessBoard.GetPieceOnSquare(square); // temp piece to use after simulating the move
                ChessBoard.UpdateBoardOnPieceMove(square, piece.OccupiedSquare, piece, null);
                opponent.GenerateAllPossibleMoves();
                if (!opponent.CheckIfIsAttackingPieceOfType<T>())
                {
                    ChessBoard.UpdateBoardOnPieceMove(piece.OccupiedSquare, square, piece, pieceOnSquare);
                    return true;
                }
                else
                {
                    ChessBoard.UpdateBoardOnPieceMove(piece.OccupiedSquare, square, piece, pieceOnSquare);
                }
            }
        }

        return false;
    }
}
