using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu (menuName = "Scriptable Objects / Events / Bool Event Channel SO")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> onEventRaised;

    public void RaiseEvent(bool value)
    {
        if (onEventRaised != null)
        {
            onEventRaised.Invoke(value);
        }
    }
}
