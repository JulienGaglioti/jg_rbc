using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBoxUI : MonoBehaviour
{
    public StringEventChannelSO infoBoxChannelSO;
    public TextMeshProUGUI infoBoxText;

    private void OnEnable()
    {
        if (infoBoxChannelSO != null)
            infoBoxChannelSO.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (infoBoxChannelSO != null)
            infoBoxChannelSO.OnEventRaised -= Respond;
    }

    public void Respond(string s)
    {
        infoBoxText.text = s;
    }
}
