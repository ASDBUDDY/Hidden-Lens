using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsInformationScript : InformationBoxScript
{
   
    protected override void SetText() => DialogText.text = DisplayStrings[(int)GameManager.Instance.CurrentPlayerControlScheme];
    
}
