
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackActionState : EnemyActionBaseState
{
    public override EnemyActionStateEnum Type => EnemyActionStateEnum.Attack;

    public EnemyAttackActionState(EnemyActionStateMachine stateMachine) : base(stateMachine)
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
