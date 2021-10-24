using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0)
    };

    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + directions[i];
            Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
            if (!ChessBoard.CheckIfCoordinatesAreOnBoard(nextCoords))
                continue;
            if (piece == null || !piece.IsFromSameTeam(this))
                TryToAddMove(nextCoords);
        }

        return availableMoves;
    }
}
