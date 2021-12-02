using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EmptyEventListener : MonoBehaviour
{
    [SerializeField] private EmptyEventChannelSO channel;

    public UnityEvent onEventRaised;

    private void OnEnable()
    {
        if (channel != null)
            channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (channel != null)
            channel.OnEventRaised -= Respond;
    }

    private void Respond()
    {
        if (onEventRaised != null)
            onEventRaised.Invoke();
    }
}