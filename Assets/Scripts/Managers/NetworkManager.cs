using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameInitializer gameInitializer;
    [SerializeField] private EmptyEventChannelSO opponentLeft;
    private MultiPlayerController _multiPlayerController;
    
    private const string LEVEL = "level";
    private const string TEAM = "team";
    private const byte MAX_PLAYERS = 2;
    private ChessLevel playerLevel;
    private string _roomName;
    
    public void Connect()
    {
        if (PhotonNetwork.IsConnected && _roomName!= "")
        {
            //PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }, MAX_PLAYERS);
            JoinOrCreateRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        uiManager.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            CustomRoomPropertiesForLobby = new string[] { LEVEL },
            MaxPlayers = MAX_PLAYERS,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }
        };
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, null);
    }

    public void SetDependencies(MultiPlayerController controller)
    {
        _multiPlayerController = controller;
    }

    public void SetRoomName(string name)
    {
        _roomName = name;
    }

    private void PrepareTeamSelectionOptions()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
            if (player.CustomProperties.ContainsKey(TEAM))
            {
                var occupiedTeam = player.CustomProperties[TEAM];
                uiManager.RestrictTeamChoice((TeamColor)occupiedTeam);
            }
        }
    }

    private void DelayedSetPlayerTeam()
    {
        if (IsRoomFull())
        {
            SetPlayerTeam(0);
        }
        else
        {
            SetPlayerTeam(1);
        }
    }

    public void SetPlayerTeam(int team)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        gameInitializer.InitializeMultiplayerController();
        _multiPlayerController.SetLocalPlayer((TeamColor)team);
        _multiPlayerController.StartNewGame();
        if (GameManager.Instance.makePiecesInvisible)
        {
            _multiPlayerController.MakeEnemyPiecesInvisible();
        }
        
        GameManager.Instance.playingAsBlack = team == 0;
        if (!GameManager.Instance.flipCameraInMultiplayer)
        {
            GameManager.Instance.playingAsBlack = false;
        }
        
        _multiPlayerController.SetupCamera((TeamColor)team);
        if (team == 1) // white
        {
            _multiPlayerController.SetTurnState(ChessGameController.TurnState.Sense);
        }
        else
        {
            _multiPlayerController.SetTurnState(ChessGameController.TurnState.Wait);
        }
    }

    public void SetPlayerLevel(ChessLevel level)
    {
        playerLevel = level;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { LEVEL, level } });
    }

    public bool IsRoomFull()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == 2;
    }

    #region Callbacks

    public override void OnJoinedRoom()
    {
        //Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room.");
        gameInitializer.CreateMultiPlayerBoard();
        
        Invoke("DelayedSetPlayerTeam", 0.2f);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.LogError($"Player {newPlayer.ActorNumber} entered the room.");
        _multiPlayerController.gameStarted.RaiseEvent();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        opponentLeft.RaiseEvent();
    }

    #endregion
}
