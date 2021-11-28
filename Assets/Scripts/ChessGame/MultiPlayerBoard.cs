using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MultiPlayerBoard : Board
{
    private PhotonView _photonView;

    protected override void Awake()
    {
        base.Awake();
        _photonView = GetComponent<PhotonView>();
    }

    public override void SelectedPieceMoved(Vector2 coords)
    {
        _photonView.RPC(nameof(RPC_OnSelectedPieceMoved), RpcTarget.AllBuffered, new object[] { coords });
    }

    [PunRPC]
    private void RPC_OnSelectedPieceMoved(Vector2 coords)
    {
        Debug.LogError("RPC - On Move");

        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
        OnSelectedPieceMoved(intCoords);
    }

    public override void SetSelectedPiece(Vector2 coords)
    {
        _photonView.RPC(nameof(RPC_SetSelectedPiece), RpcTarget.AllBuffered, new object[] { coords });
    }

    [PunRPC]
    private void RPC_SetSelectedPiece(Vector2 coords)
    {
        Debug.LogError("RPC - On Select");

        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
        OnSetSelectedPiece(intCoords);
    }

    
}
