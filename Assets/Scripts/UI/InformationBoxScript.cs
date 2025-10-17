using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationBoxScript : MonoBehaviour
{
    public CanvasGroup DialogBox;
    public TextMeshProUGUI DialogText;
    public List<string> DisplayStrings;
    [SerializeField]
    private float loadSpeed = 2f;
    private Coroutine DialogRoutine;
    protected virtual void SetText()
    {
        DialogText.text = DisplayStrings[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            SetText();
            if(DialogRoutine != null)
                StopCoroutine(DialogRoutine);
            DialogRoutine = StartCoroutine(DialogLoad(true));
        }
    }

    private IEnumerator DialogLoad(bool flag)
    {
        float alpha = 0f;
        if (flag)
        {
            alpha = 0f;

            while (alpha < 1.0f)
            {
                alpha += TimeManager.Instance.DeltaTime * loadSpeed;
                
                    if(alpha>1f)
                        alpha = 1.0f;
                
                DialogBox.alpha = alpha;
                yield return new WaitForEndOfFrame();
            }

        }
        else
        {
            alpha = 1f;
            while (alpha > 0f)
            {
                alpha -= TimeManager.Instance.DeltaTime * loadSpeed;

                if(alpha<0f)
                    alpha = 0f;

                DialogBox.alpha = alpha;
                yield return new WaitForEndOfFrame();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            if (DialogRoutine != null)
                StopCoroutine(DialogRoutine);
            DialogRoutine = StartCoroutine(DialogLoad(false));
        }
    }
}
