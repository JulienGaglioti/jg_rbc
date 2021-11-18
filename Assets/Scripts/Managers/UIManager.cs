using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private TextMeshProUGUI winnerText;

    public void OnGameFinished(string winnerName)
    {
        endGameScreen.SetActive(true);
        winnerText.SetText(winnerName + " won!");
    }

    public void HideEndGameScreen()
    {
        endGameScreen.SetActive(false);
    }
}
