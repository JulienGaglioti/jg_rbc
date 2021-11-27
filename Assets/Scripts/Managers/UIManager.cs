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

    [Header("Other UI")]
    [SerializeField] private TMP_Dropdown gameLevelSelection;

    private void Awake()
    {
        gameLevelSelection.AddOptions(Enum.GetNames(typeof(ChessLevel)).ToList());
        OnGameLaunched();
    }
    private void OnGameLaunched()
    {
        DisableAllScreens();
        gameModeSelectionScreen.SetActive(true);
    }

    public void OnSinglePlayerModeSelected()
    {
        DisableAllScreens();
    }

    public void OnMultiPlayerModeSelected()
    {
        connectionStatusText.gameObject.SetActive(true);
        DisableAllScreens();
        connectScreen.SetActive(true);
    }

    public void OnConnect()
    {
        networkManager.SetPlayerLevel((ChessLevel)gameLevelSelection.value);
        networkManager.Connect();
    }

    internal void ShowTeamSelectionScreen()
    {
        DisableAllScreens();
        teamSelectionScreen.SetActive(true);
    }

    public void SelectTeam(int team)
    {
        Debug.LogError($"Selected Team {team}");
        networkManager.SetPlayerTeam(team);
    }

    public void SetConnectionStatus(string status)
    {
        connectionStatusText.SetText(status);
    }

    private void DisableAllScreens()
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
}
