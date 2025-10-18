using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBaseScript : MonoBehaviour
{
    public bool IsActivated = false;

    protected virtual void Start() { }
    public virtual void OnActivate()
    {
     IsActivated = true;   
    }
}
