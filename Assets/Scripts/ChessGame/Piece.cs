using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
    [SerializeField] private MaterialSetter materialSetter;
    public Board Board { protected get; set; }
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
        materialSetter = GetComponent<MaterialSetter>();
        HasMoved = false;
    }

    public void SetMaterial(Material selectedMaterial)
    {
        materialSetter.SetSingleMaterial(selectedMaterial);
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

    }

    protected void TryToAddMove(Vector2Int coords)
    {
        availableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords, TeamColor team, Board board)
    {
        Team = team;
        OccupiedSquare = coords;
        Board = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }
}
