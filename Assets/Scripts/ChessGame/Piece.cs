using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
    private TeamColorSetter teamColorSetter;
    public Board ChessBoard { protected get; set; }
    public Vector2Int OccupiedSquare { get; set; }
    public TeamColor Team { get; set; }
    public bool HasMoved { get; private set; }
    public List<Vector2Int> availableMoves;

    private IObjectTweener tweener;

    public abstract List<Vector2Int> SelectAvailableSquares();

    private void Awake()
    {
        availableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        teamColorSetter = GetComponent<TeamColorSetter>();
        HasMoved = false;
    }

    public abstract Vector2Int AttemptMove(Vector2Int coords);

    public void SetTeamColor(TeamColor color)
    {
        teamColorSetter.SetColorByTeam(color);
    }

    protected Vector2Int GetNormalizedDirection(Vector2Int direction)
    {
        int x = direction.x;
        int y = direction.y;
        
        if (x != 0)
        {
            x = x / Mathf.Abs(x);
        }
        if (y != 0)
        {
            y = y / Mathf.Abs(y);
        }

        return new Vector2Int(x, y);
    }

    public void MakeInvisible()
    {
        //GetComponent<MeshRenderer>().enabled = false;
        
        teamColorSetter.MakeInvisible();
    }

    public bool IsFromSameTeam(Piece piece)
    {
        return (Team == piece.Team);
    }

    public bool CanMoveTo(Vector2Int coords)
    {
        return availableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {
        Vector3 targetPosition = ChessBoard.CalculatePositionFromCoords(coords);
        OccupiedSquare = coords;
        HasMoved = true;
        tweener.MoveTo(transform, targetPosition);
    }

    protected void TryToAddMove(Vector2Int coords)
    {
        availableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords, TeamColor team, Board board)
    {
        Team = team;
        OccupiedSquare = coords;
        ChessBoard = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }

    public bool IsAttackingPieceOfType<T>() where  T : Piece
    {
        foreach (var square in availableMoves)
        {
            if (ChessBoard.GetPieceOnSquare(square) is T)
            {
                return true;
            }
        }

        return false;
    }
}
