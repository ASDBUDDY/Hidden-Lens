using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GoblinEnemyScript : EnemyBaseScript
{
    [Header("Goblin Specific")]
    private BoxCollider2D enemyCollider;
    private Vector2 colliderOffset = new Vector2(0f, -0.06917371f);
    private Vector2 dodgeColliderOffset = new Vector2(-0.2f, -0.06917371f);

    public Transform AttackPoint2;
    public Vector2 AttackSize2;

    public override void Awake()
    {
        base.Awake();
        enemyCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    public override void Start()
    {
        EnemyType = EnemyTypeEnum.Goblin;
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

            
        }
        if (hitCheck == null || attackType > 0)
        {
            if (attackType >= mainStats.DamageData.Count - 1)
            {
                attackType = 0;
                attackTimer = mainStats.AttackSpeed;
            }
            else
            {
                attackType++;
            }
        }

        Invoke(nameof(ReturnToChase), 1f);

    }

    public void SecondAttackDodge(int start = 0)
    {
        if (start == 0)
        {
            enemyCollider.offset = dodgeColliderOffset;
        }
        else
        {
            enemyCollider.offset =colliderOffset;
        }
    }


    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireCube(AttackPoint2.position, AttackSize2);
    }
}
