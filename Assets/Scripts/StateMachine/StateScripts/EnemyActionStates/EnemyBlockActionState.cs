
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockActionState : EnemyActionBaseState
{
    public override EnemyActionStateEnum Type => EnemyActionStateEnum.Block;

    public EnemyBlockActionState(EnemyActionStateMachine stateMachine, EnemyBaseScript baseClass) : base(stateMachine, baseClass)
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
