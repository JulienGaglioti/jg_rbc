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
    [SerializeField] private SenseManager senseManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraSetup cameraSetup;
    [SerializeField] private PassButtonDependency passButtonDependency;
    [SerializeField] private List<SensePlatformInputHandler> platforms;

    [Header("Events")] 
    public EmptyEventChannelSO dependenciesSet;

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
        
        controller.SetDependencies(uiManager, board, cameraSetup);
        controller.CreatePlayers();
        controller.SetNetworkManager(networkManager);
        
        networkManager.SetDependencies(controller);
        senseManager.SetDependencies(board, controller);
        passButtonDependency.SetController(controller);
        board.SetDependencies(controller, senseManager);
        SetPlatformsDependencies(board);
        dependenciesSet.RaiseEvent();
    }

    public void InitializeSinglePlayerController()
    {
        SinglePlayerBoard board = FindObjectOfType<SinglePlayerBoard>();
        SinglePlayerController controller = Instantiate(spControllerPrefab);
        
        controller.SetDependencies(uiManager, board, cameraSetup);
        controller.CreatePlayers();
        board.SetDependencies(controller, senseManager);
        senseManager.SetDependencies(board, controller);
        controller.StartNewGame();
    }

    private void SetPlatformsDependencies(Board board)
    {
        foreach (var platform in platforms)
        {
            platform.SetBoard(board);
        }
    }
}
