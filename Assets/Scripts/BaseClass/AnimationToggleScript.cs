using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToggleScript : MonoBehaviour
{
    public bool ToggleSpeed = false;
    public float AnimSpeed = 1f;

    private Animator animatorObj;

    private void Awake()
    {
        animatorObj = GetComponent<Animator>();
    }
    private void Start()
    {
        if(ToggleSpeed)
        animatorObj.speed = AnimSpeed;
    }

    public void DisableObj()
    {
        this.gameObject.SetActive(false);
    }
}
