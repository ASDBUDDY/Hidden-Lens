using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLensActiveObject : HiddenLensObjects
{
    public SpriteRenderer SpriteVisual;


    protected override void Start()
    {
        ToggleReveal(false);
    }

    public override void ToggleReveal(bool value)
    {
        RevealFlag = value;

        Color newColor = SpriteVisual.color;
        newColor.a = value ? 1f : 0f;
        SpriteVisual.color = newColor;
    }

}
