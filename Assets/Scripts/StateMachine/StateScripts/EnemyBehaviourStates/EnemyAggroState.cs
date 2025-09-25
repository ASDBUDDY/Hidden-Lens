
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
