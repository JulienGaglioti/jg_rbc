using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    private Vector2Int[] offsets = new Vector2Int[]
    {
        new Vector2Int(2, 1),
        new Vector2Int(2, -1),
        new Vector2Int(1, 2),
        new Vector2Int(1, -2),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1),
        new Vector2Int(-1, 2),
        new Vector2Int(-1, -2)
    };
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + offsets[i];
            Piece piece = ChessBoard.GetPieceOnSquare(nextCoords);
            if(!ChessBoard.CheckIfCoordinatesAreOnBoard(nextCoords))
                continue;
            if (piece == null || !piece.IsFromSameTeam(this))
                TryToAddMove(nextCoords);
        }

        return availableMoves;
    }

    public override Vector2Int AttemptMove(Vector2Int coords)
    {
        return coords;
    }
}
