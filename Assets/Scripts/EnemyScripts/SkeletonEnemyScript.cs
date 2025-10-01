using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkeletonEnemyScript : EnemyBaseScript
{
    [Header("Skeleton Specific")]
    private float blockTimer =0f;
    [SerializeField] private float blockTime =1f;
    private BoxCollider2D enemyCollider;
    private Vector2 colliderOffset = new Vector2(-0.05655462f, -0.000712961f);
    private Vector2 blockColliderOffset = new Vector2(0.1f, -0.000712961f);


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
        base.Start();
        EnemyType = EnemyTypeEnum.Skeleton;
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

     public void ReturnOnBlock()
     {
        Invoke(nameof(ReturnToChase), 1f);
     }
    public override void ToggleBlock(bool flag = true)
    {
        if (flag)
        {
            
            
            enemyAnimatorScript.TriggerBlock();

            float diffPosition = transform.position.x - TargetObj.transform.position.x;

            SetRotationAndVelocity(diffPosition);
            enemyAnimatorScript.SetVelocity(0f);
        }
       
    }

    public override void CheckForAttack()
    {
        if (attackTimer <= 0f && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Attack && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Hurt)
        {
            if (TargetObj != null)
            {
                float range = Mathf.Abs(transform.position.x - TargetObj.transform.position.x);
                for (int i = 0; i < mainStats.AttackRange.Count; i++)
                {
                    if (range >= mainStats.AttackRange[i].x && range <= mainStats.AttackRange[i].y)
                    {
                        CancelInvoke(nameof(ReturnToChase));

                        actionStateMachine.SetState(EnemyActionStateEnum.Attack);
                        attackType = i;
                        attackTimer = mainStats.AttackSpeed;
                        break;

                    }
                }
            }
        }
        else
        {
            attackTimer -= TimeManager.Instance.DeltaTime;

            if (TargetObj != null)
            {
                if (blockTimer <=0f && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Block && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Attack && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Hurt)
                {
                    float range = Vector3.Distance(transform.position, TargetObj.transform.position);
                    if (range >= mainStats.AttackRange[0].x && range <= mainStats.AttackRange[0].y)
                    {
                        actionStateMachine.SetState(EnemyActionStateEnum.Block);
                        blockTimer = blockTime;
                    }
                }
                else
                {
                    blockTimer -= TimeManager.Instance.DeltaTime;
                }
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireCube(AttackPoint2.position, AttackSize2);
    }
}
