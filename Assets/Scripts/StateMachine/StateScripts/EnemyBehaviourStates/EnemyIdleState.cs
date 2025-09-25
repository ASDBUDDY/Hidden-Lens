
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override EnemyStateEnum Type => EnemyStateEnum.Idle;

    public EnemyIdleState(EnemyStateMachine stateMachine,EnemyBaseScript baseClass) : base(stateMachine,baseClass)
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
        MovementFunction();


    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
