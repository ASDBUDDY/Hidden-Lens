using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyScript : EnemyBaseScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        EnemyType = EnemyTypeEnum.Slime;
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
}
