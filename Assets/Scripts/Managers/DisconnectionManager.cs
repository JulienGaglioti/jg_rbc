using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectionManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameInitializer gameInitializer;
    [SerializeField] private ColliderInputReceiver colliderInputReceiver;
    
    public void OnDisconnect()
    {
        uiManager.ResetInitialUI();
        uiManager.ResetRoomName();
        uiManager.EnableStatusScreen(true);
        gameInitializer.ActivateSensePlatforms(false);
        colliderInputReceiver.enabled = false;
        var controller = FindObjectOfType<ChessGameController>();
        var board = FindObjectOfType<Board>();
        if (controller)
        {
            Destroy(controller.gameObject);
        }
        if (board)
        {
            Destroy(board.gameObject);
        }
    }
}
