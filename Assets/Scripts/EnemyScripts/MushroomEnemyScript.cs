using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MushroomEnemyScript : EnemyBaseScript
{

    [Header("Mushroom Specific")]

    public Transform AttackPoint2;
    public Vector2 AttackSize2;

    public override void Awake()
    {
        base.Awake();
        
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        EnemyType = EnemyTypeEnum.Mushroom;
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
        Vector3 attackPos = Vector3.zero;
        Vector2 attackSize = Vector2.zero;

        switch (attackType)
        {
            case 0:
                attackPos = AttackPoint.position;
                attackSize = AttackSize; break;
            case 1: attackPos = AttackPoint2.position;
                attackSize = AttackSize2; break;
            default:
                attackPos = AttackPoint.position;
                attackSize = AttackSize; break;

        }

        Collider2D hitCheck = Physics2D.OverlapBox(attackPos, attackSize, 0, DetectionLayerMask);
        if (hitCheck != null)
        {
            PlayerMainScript newPlayer = hitCheck.GetComponent<PlayerMainScript>();
            newPlayer.OnDamage(mainStats.DamageData[attackType]);
            attackTimer = mainStats.AttackSpeed;

            if(attackType == 1)
            {
                enemyHealth.IncreaseHealth(mainStats.DamageData[attackType]);
            }
            
        }
       

        Invoke(nameof(ReturnToChase), 1f);

    }


    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireCube(AttackPoint2.position, AttackSize2);
    }
}
