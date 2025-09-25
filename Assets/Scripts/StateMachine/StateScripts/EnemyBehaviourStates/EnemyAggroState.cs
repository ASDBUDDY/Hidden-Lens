
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public override EnemyStateEnum Type => EnemyStateEnum.Aggro;

    public EnemyAggroState(EnemyStateMachine stateMachine, EnemyBaseScript baseClass) : base(stateMachine, baseClass)
    {

    }
    internal override void OnEnter()
    {
        base.OnEnter();
       SetActionState(EnemyActionStateEnum.Chase);

    }
    internal override void OnExit()
    {
        base.OnExit();
        SetActionState(EnemyActionStateEnum.Idle);
    }
    internal override void Update()
    {

        base.Update();

        DetectionFunction();
        AttackDetection();

    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
