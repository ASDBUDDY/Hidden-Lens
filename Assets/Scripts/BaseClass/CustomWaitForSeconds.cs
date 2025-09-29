using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWaitForSeconds : CustomYieldInstruction
{
    private float secondLength;
    private float startTime;
    public CustomWaitForSeconds(float seconds)
    {
        startTime = TimeManager.Instance.TimeInSeconds;
        secondLength = seconds;
    }
    public override bool keepWaiting
    {
        get
        {
            return TimeManager.Instance.TimeInSeconds - startTime < secondLength;
        }
    }

}
