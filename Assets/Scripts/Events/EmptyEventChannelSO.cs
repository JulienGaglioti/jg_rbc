using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Events/Empty Event")]
public class EmptyEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    [ContextMenu("Raise")]
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}
