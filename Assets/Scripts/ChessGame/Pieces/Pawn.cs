using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();

        Vector2Int direction = Team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = HasMoved ? 1 : 2;
        
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + direction * i;
            Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
            if (!ChessBoard.CheckIfCoordinatesAreOnBoard(nextCoords))
            {
                break;
            }
            if (piece == null)
            {
                TryToAddMove(nextCoords);
            }
            else if (!piece.IsFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
            else
            {
                break;
            }
        }

        Vector2Int[] takeDirections = new Vector2Int[]
            { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + takeDirections[i];
            Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
            if (!ChessBoard.CheckIfCoordinatesAreOnBoard(nextCoords))
                continue;
            if (piece == null)
            {
                TryToAddMove(nextCoords);
            }
            else if (!piece.IsFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }

        return availableMoves;
    }

    public override Vector2Int AttemptMove(Vector2Int coords)
    {
        Vector2Int realCoords = coords;
        if (coords.x != OccupiedSquare.x)
        {
            // The pawn is trying to attack
            Piece piece = ChessBoard.GetPieceOnSquare(coords);
            if (piece!= null && !piece.IsFromSameTeam(this))
            {
                realCoords = piece.OccupiedSquare;
            }
            else
            {
                realCoords = OccupiedSquare;
            }
        }
        else
        {
            Vector2Int direction = Team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
            float range = HasMoved ? 1 : 2;

            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = OccupiedSquare + direction * i;
                Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);

                if (piece != null)
                {
                    realCoords = OccupiedSquare;
                    break;
                }
            }
        }
        return realCoords;
    }
}
