using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyScript : EnemyBaseScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        EnemyType = EnemyTypeEnum.Slime;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MovementFunction()
    {
        base.MovementFunction();
    }

    public override void OnAttack()
    {
        base.OnAttack();

        SetActionState(EnemyActionStateEnum.Chase);
    }
}
