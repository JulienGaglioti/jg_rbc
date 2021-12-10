using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MultiPlayerController : ChessGameController, IOnEventCallback
{
    protected const byte SET_GAME_STATE_EVENT_CODE = 1;
    private NetworkManager _networkManager;
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    protected override void SetGameState(GameState state)
    {
        object[] content = new object[] { (int)state };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SET_GAME_STATE_EVENT_CODE, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public override void TryToStartCurrentGame()
    {
        if (_networkManager.IsRoomFull())
        {
            SetGameState(GameState.Play);
            gameStarted.RaiseEvent();
        }
    }

    public override bool CanPerformMove()
    {
        if (!IsGameInProgress() || !IsLocalPlayersTurn())
        {
            return false;
        }

        return true;
    }

    protected override void ChangeActiveTeam()
    {
        _activePlayer = _activePlayer == _whitePlayer ? _blackPlayer : _whitePlayer;
        if (turnState == TurnState.Move)
        {
            SetTurnState(TurnState.Wait);
        }
        else
        {
            SetTurnState(TurnState.Sense);
        }
    }

    public void SetNetworkManager(NetworkManager networkManager)
    {
        _networkManager = networkManager;
    }

    public void MakeEnemyPiecesInvisible()
    {
        foreach (var piece in GetOpponentToPlayer(_localPlayer).ActivePieces)
        {
            piece.MakeInvisible();
        }
    }

    public bool IsLocalPlayersTurn()
    {
        return _localPlayer == _activePlayer;
    }

    public void SetLocalPlayer(TeamColor team)
    {
        _localPlayer = team == TeamColor.White ? _whitePlayer : _blackPlayer;
    }

    public void OnEvent(EventData photonEvent)
    {
        // response to event used to change state
        byte eventCode = photonEvent.Code;
        if (eventCode == SET_GAME_STATE_EVENT_CODE)
        {
            object[] data = (object[])photonEvent.CustomData;
            GameState state = (GameState)data[0];
            _gameState = state;
        }
    }
}
