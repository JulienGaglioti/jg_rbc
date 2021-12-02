using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Game mode dependent objects")]
    [SerializeField] private SinglePlayerController spControllerPrefab;
    [SerializeField] private MultiPlayerController mpControllerPrefab;
    [SerializeField] private MultiPlayerBoard mpBoardPrefab;
    [SerializeField] private SinglePlayerBoard spBoardPrefab;

    [Header("Scene references")] 
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private UIManager uiManager;

    public void CreateMultiPlayerBoard()
    {
        if (!networkManager.IsRoomFull())
        {
            PhotonNetwork.Instantiate(mpBoardPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    public void CreateSinglePlayerBoard()
    {
        Instantiate(spBoardPrefab);
    }

    public void InitializeMultiplayerController()
    {
        MultiPlayerBoard board = FindObjectOfType<MultiPlayerBoard>();
        MultiPlayerController controller = Instantiate(mpControllerPrefab);
        
        controller.SetDependencies(uiManager, board);
        controller.CreatePlayers();
        controller.SetNetworkManager(networkManager);
        
        networkManager.SetDependencies(controller);
        board.SetDependencies(controller);
    }

    public void InitializeSinglePlayerController()
    {
        SinglePlayerBoard board = FindObjectOfType<SinglePlayerBoard>();
        SinglePlayerController controller = Instantiate(spControllerPrefab);
        
        controller.SetDependencies(uiManager, board);
        controller.CreatePlayers();
        board.SetDependencies(controller);
        controller.StartNewGame();
    }
}
