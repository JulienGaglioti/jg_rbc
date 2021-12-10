using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
    };
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        float range = Board.BOARD_SIZE;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = OccupiedSquare + direction * i;
                Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
                if (!ChessBoard.CheckIfCoordinatesAreOnBoard(nextCoords))
                    break;
                if (piece == null)
                    TryToAddMove(nextCoords);
                else if (!piece.IsFromSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                }
                else if (piece.IsFromSameTeam(this))
                {
                    break;
                }
            }
        }

        return availableMoves;
    }

    public override Vector2Int AttemptMove(Vector2Int coords)
    {
        var direction = GetNormalizedDirection(coords - OccupiedSquare);
        int range = Mathf.Max(Mathf.Abs(coords.x - OccupiedSquare.x), Mathf.Abs(coords.y - OccupiedSquare.y));
        print(range);
        Vector2Int realCoords = coords;

        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + direction * i;
            Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
            
            if (piece != null)
            {
                if (!piece.IsFromSameTeam(this))
                {
                    realCoords = nextCoords;
                    break;
                }
            }
        }

        return realCoords;
    }
}
