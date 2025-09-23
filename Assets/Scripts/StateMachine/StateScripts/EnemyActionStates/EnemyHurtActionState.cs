
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtActionState : EnemyActionBaseState
{
    public override EnemyActionStateEnum Type => EnemyActionStateEnum.Hurt;

    public EnemyHurtActionState(EnemyActionStateMachine stateMachine) : base(stateMachine)
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
