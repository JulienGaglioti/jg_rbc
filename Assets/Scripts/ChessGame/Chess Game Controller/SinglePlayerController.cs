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
    }

    public override bool CanPerformMove()
    {
        if (!IsGameInProgress())
        {
            return false;
        }

        return true;
    }
}
