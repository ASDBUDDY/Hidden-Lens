using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLensObjects : MonoBehaviour
{

    public bool RevealFlag = false;
    private bool collidingFlag = false;

    public GameObject HiddenObj;


    // Start is called before the first frame update
    void Start()
    {
        HiddenObj.SetActive(false);
        RevealFlag = false;
        collidingFlag = false;
    }

    public void ToggleReveal(bool value)
    {
        RevealFlag =value;
        if (collidingFlag)
            return;

        HiddenObj.SetActive(RevealFlag);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            if (!RevealFlag)
            {
                collidingFlag=true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            collidingFlag=false;

            if(RevealFlag)
                HiddenObj.SetActive(RevealFlag);
        }
    }
}
