using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{

    public static DeathUI Instance;

    public GameObject Vignette;
    public GameObject MainBG;
    public Button Retry;

    
 

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

    }



    public void ToggleUI(bool flag)
    {

        if (flag)
        {
            MainBG.SetActive(true);
            Vignette.SetActive(true);
            Retry.Select();
        }
        else
        {
            MainBG.SetActive(false);
            Vignette.SetActive(false);
            
        }
    }

            
}
