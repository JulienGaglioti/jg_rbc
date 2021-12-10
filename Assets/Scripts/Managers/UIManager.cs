using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Dependencies")] 
    [SerializeField] private NetworkManager networkManager;

    [Header("Buttons")] 
    [SerializeField] private Button whiteTeamButton;
    [SerializeField] private Button blackTeamButton;

    [Header("Texts")] 
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI connectionStatusText;

    [Header("Screen Game Objects")] 
    [SerializeField] private GameObject gameModeSelectionScreen;
    [SerializeField] private GameObject connectScreen;
    [SerializeField] private GameObject teamSelectionScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject statusScreen;

    [Header("In Game Objects")] 
    [SerializeField] private GameObject passButton;
    [SerializeField] private GameObject switchCameraButton;
    [SerializeField] private GameObject infoBox;
    

    [Header("Other UI")]
    [SerializeField] private TMP_Dropdown gameLevelSelection;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TextMeshProUGUI roomName;

    private void Awake()
    {
        // gameLevelSelection.AddOptions(Enum.GetNames(typeof(ChessLevel)).ToList());
        OnGameLaunched();
    }
    private void OnGameLaunched()
    {
        DisableMenuScreens();
        gameModeSelectionScreen.SetActive(true);
    }

    public void OnChessGameStarted()
    {
        DisableMenuScreens();
        connectionStatusText.gameObject.SetActive(false);
    }

    public void OnSinglePlayerModeSelected()
    {
        DisableMenuScreens();
    }

    public void OnMultiPlayerModeSelected()
    {
        DisableMenuScreens();
        connectionStatusText.gameObject.SetActive(true);
        connectScreen.SetActive(true);
    }

    public void OnConnect()
    {
        networkManager.SetRoomName(roomNameInputField.text);
        roomName.SetText(roomNameInputField.text);
        networkManager.Connect();
    }

    internal void ShowTeamSelectionScreen()
    {
        DisableMenuScreens();
        teamSelectionScreen.SetActive(true);
    }

    public void SelectTeam(int team)
    {
        //Debug.LogError($"Selected Team {team}");
        networkManager.SetPlayerTeam(team);
    }

    public void SetConnectionStatus(string status)
    {
        connectionStatusText.SetText(status);
    }

    private void DisableMenuScreens()
    {
        gameOverScreen.SetActive(false);
        teamSelectionScreen.SetActive(false);
        connectScreen.SetActive(false);
        gameModeSelectionScreen.SetActive(false);
    }

    public void RestrictTeamChoice(TeamColor occupiedTeam)
    {
        Button buttonToDeactivate = occupiedTeam == TeamColor.White ? whiteTeamButton : blackTeamButton;
        buttonToDeactivate.interactable = false;
    }


    public void OnChessGameFinished(string winner)
    {
        gameOverScreen.SetActive(true);
        resultText.SetText(winner + " won");
    }

    public void EnableInGameObjects()
    {
        switchCameraButton.SetActive(true);
        passButton.SetActive(true);
        infoBox.SetActive(true);
    }

    public void EnableStatusScreen(bool b)
    {
        statusScreen.SetActive(b);
    }

    public void ResetInitialUI()
    {
        DisableMenuScreens();
        switchCameraButton.SetActive(false);
        passButton.SetActive(false);
        infoBox.SetActive(false);
        gameModeSelectionScreen.SetActive(true);
    }

    public void ResetRoomName()
    {
        roomName.SetText("Room Name");
        networkManager.SetRoomName("");
    }
    
}
