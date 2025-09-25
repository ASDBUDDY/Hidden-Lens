
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseActionState : EnemyActionBaseState
{
    public override EnemyActionStateEnum Type => EnemyActionStateEnum.Chase;

    public EnemyChaseActionState(EnemyActionStateMachine stateMachine, EnemyBaseScript baseClass) : base(stateMachine,baseClass)
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
        ChaseFunction();


    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
