using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Events/String Event")]
public class StringEventChannelSO : ScriptableObject
{
    public UnityAction<string> OnEventRaised;

    [ContextMenu("Raise")]
    public void RaiseEvent(string s)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(s);
    }
}