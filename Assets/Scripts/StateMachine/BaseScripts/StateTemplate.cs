
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTemplate : BaseStateTemplate
{
    public override StateEnum Type => StateEnum.Default;

    public StateTemplate(StateMachineTemplate stateMachine) : base(stateMachine)
    {

    }
    internal override void OnEnter()
    {
        base.OnEnter();
       

    }
    internal override void OnExit()
    {
        base.OnExit();

    }
    internal override void Update()
    {

        base.Update();
        


    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
