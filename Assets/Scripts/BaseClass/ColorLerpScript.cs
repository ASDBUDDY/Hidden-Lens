using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerpScript : MonoBehaviour
{

    public Color StartColor;
    public Color EndColor;

    public bool IsActive = false;

    private Image LerpImage;
    private Coroutine LerpRoutine;
    public float LerpBounce = 2f;

    private void Awake()
    {
        LerpImage = GetComponent<Image>();
    }

    public void ToggleColorLerp(bool active)
    {
        IsActive =active;
        if(LerpRoutine != null) 
            StopCoroutine(LerpRoutine);

        if (IsActive)
        {
            LerpRoutine = StartCoroutine(ColorLerping());
        }
        else
        {
            LerpImage.color = StartColor;
        }
    }
    private IEnumerator ColorLerping()
    {
        while (IsActive)
        {
            LerpImage.color = Color.Lerp(StartColor, EndColor,Mathf.PingPong(TimeManager.Instance.TimeInSeconds,1));

            yield return new CustomWaitForSeconds(LerpBounce);
        }
        yield return null;
    }
}
