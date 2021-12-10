using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerController : ChessGameController
{
    protected override void SetGameState(GameState state)
    {
        _gameState = state;
    }

    public override void TryToStartCurrentGame()
    {
        SetGameState(GameState.Play);
        gameStarted.RaiseEvent();
    }

    public override bool CanPerformMove()
    {
        if (!IsGameInProgress())
        {
            return false;
        }

        return true;
    }

    protected override void ChangeActiveTeam()
    {
        _activePlayer = _activePlayer == _whitePlayer ? _blackPlayer : _whitePlayer;
    }
}
