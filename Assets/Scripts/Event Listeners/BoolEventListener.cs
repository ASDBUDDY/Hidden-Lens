using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolEventListener : MonoBehaviour
{
    [SerializeField] private BoolEventChannelSO eventChannel = default;
    public UnityEvent<bool> onEventRaised;

    private void OnEnable()
    {
        if (onEventRaised != null)
        {
            eventChannel.onEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (onEventRaised != null)
        {
            eventChannel.onEventRaised -= Respond;
        }
    }

    private void Respond(bool value)
    {
        if (onEventRaised != null)
        {
            onEventRaised.Invoke(value);
        }
    }
}
