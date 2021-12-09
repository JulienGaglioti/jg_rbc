using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnStateUI : MonoBehaviour
{
    public ChessGameController controller;
    public TextMeshProUGUI turnStateText;

    public void OnEnable()
    {
        controller = FindObjectOfType<ChessGameController>();
    }
    private void Update()
    {
        turnStateText.text = controller.turnState.ToString();
    }
}
