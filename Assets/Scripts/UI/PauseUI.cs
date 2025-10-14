using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{

    public static PauseUI Instance;

    public GameObject Vignette;
    public Animator PauseAnimator;
    public Button Resume;

    
    public string EndAnim;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

    }



    public void ToggleUI(bool flag)
    {

        if (TimeManager.Instance.TimePaused)
        {
            PauseAnimator.gameObject.SetActive(true);
            Vignette.SetActive(true);
            Resume.Select();
        }
        else
        {
            PauseAnimator.Play(EndAnim);
            Vignette.SetActive(false);
            
        }
    }

    public void UnPauseGame()
    {
        TimeManager.Instance.PauseGame(false);
    }
            
}
