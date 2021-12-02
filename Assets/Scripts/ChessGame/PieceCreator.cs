using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;
    private Transform _piecesParentTransform;
    private Dictionary<string, GameObject> _nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _piecesParentTransform = FindObjectOfType<PiecesParent>().transform;
        foreach (var piece in piecesPrefabs)
        {
            _nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
        }
    }

    public GameObject CreatePiece(Type type)
    {
        GameObject prefab = _nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab, _piecesParentTransform);
            return newPiece;
        }

        return null;
    }

    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
}
