using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    #region Variables
    private EnemyStateMachine behaviourStateMachine;
    private EnemyActionStateMachine actionStateMachine;
    private EnemyAnimatorScript enemyAnimatorScript;
    [SerializeField]
    private EnemyStats mainStats;
    private HealthComponent enemyHealth;

    
    public List<GameObject> MovementList;
    public EnemyTypeEnum EnemyType;
    


    #region Movement Variables
    private float stopTimer = 0f;
    private int movementCounter = 0;


    #endregion

    #region Detection Variables
    public LayerMask DetectionLayerMask;
    private float detectionTimer = 0f;

    private GameObject TargetObj;

    #endregion

    #region Attack Variables
    private float attackTimer = 0f;
    private int attackType = 0;
    public Transform AttackPoint;
    public Vector2 AttackSize = Vector2.one;


    #endregion

    #endregion

    #region Base Methods
    private void Awake()
    {
        behaviourStateMachine = GetComponent<EnemyStateMachine>();
        actionStateMachine = GetComponent<EnemyActionStateMachine>();
        enemyAnimatorScript = GetComponent<EnemyAnimatorScript>();
       
    }


    // Start is called before the first frame update
   public virtual void Start()
    {
        enemyHealth = new HealthComponent(mainStats.MaxHealth);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        behaviourStateMachine.CallStateUpdate();
        actionStateMachine.CallStateUpdate();
    }
    #endregion

    #region Functionality Body
    public virtual void MovementFunction()
    {
        if(movementCounter >= MovementList.Count)
            movementCounter = 0;

        if (Vector3.Distance(transform.position, MovementList[movementCounter].transform.position) <= 0.1f)
        {
            if (stopTimer >= mainStats.StopTime)
            {
                stopTimer = 0f;
                movementCounter++;

            }
            else
            {
                stopTimer += TimeManager.Instance.DeltaTime;
            }

            enemyAnimatorScript.SetVelocity(0f);

        }
        else
        {
            
             Vector3 MovementPos = Vector3.MoveTowards(transform.position, MovementList[movementCounter].transform.position, mainStats.MovementSpeed * TimeManager.Instance.DeltaTime);

            float diffPosition = transform.position.x - MovementPos.x;
            
            SetRotationAndVelocity(diffPosition);

                transform.position = MovementPos;
        }
        
    }
    private void SetRotationAndVelocity(float diffPosition)
    {
        if (diffPosition < 0)
        {
            transform.rotation = Quaternion.Euler(transform.position.x, 180f, transform.position.z);
        }
        else if (diffPosition > 0)
        {
            transform.rotation = Quaternion.Euler(transform.position.x, 0f, transform.position.z);
        }

        float velocity =  Mathf.Abs(diffPosition/TimeManager.Instance.DeltaTime);

        enemyAnimatorScript.SetVelocity(velocity);
    }
    public virtual void DetectionFunction()
    {
        if (detectionTimer > 0.1f)
        {
            detectionTimer = 0f;

            Collider2D ColliderHit = Physics2D.OverlapBox(transform.position + transform.right * -1, mainStats.DetectionRange, 0, DetectionLayerMask);
            if (ColliderHit)
            {
                TargetObj = ColliderHit.gameObject;
                behaviourStateMachine.SetState(EnemyStateEnum.Aggro);
            }
            else if(TargetObj != null)
            {
                TargetObj = null;
                behaviourStateMachine.SetState(EnemyStateEnum.Idle);
            }

        }
        else
        {
            detectionTimer += TimeManager.Instance.DeltaTime;
        }
    }

    public virtual void ChaseFunction()
    {
        if (TargetObj != null)
        {
            if (Vector3.Distance(transform.position, TargetObj.transform.position) > mainStats.AttackRange[0].x)
            {
                Vector3 newDest = Vector3.MoveTowards(transform.position, TargetObj.transform.position, mainStats.MovementSpeed *1.1f * TimeManager.Instance.DeltaTime);
                newDest.y = transform.position.y;

                float diffPosition = transform.position.x - newDest.x;

                SetRotationAndVelocity(diffPosition);

                transform.position = newDest;
            }
        }
    }

    public virtual void CheckForAttack()
    {
        if (attackTimer <= 0f && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Attack)
        {
            if (TargetObj != null)
            {
                float range = Vector3.Distance(transform.position, TargetObj.transform.position);
                for (int i = 0; i < mainStats.AttackRange.Count; i++)
                {
                    if (range >= mainStats.AttackRange[i].x && range <= mainStats.AttackRange[i].y)
                    {
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
        }
    }

    public virtual void OnAttack()
    {
        Collider2D hitCheck = Physics2D.OverlapBox(AttackPoint.position, AttackSize,0,DetectionLayerMask);
        if (hitCheck != null)
        {
            PlayerMainScript newPlayer = hitCheck.GetComponent<PlayerMainScript>();
            newPlayer.OnDamage(mainStats.DamageData[attackType]);
        }
        attackTimer = mainStats.AttackSpeed;
    }

    public virtual void ReturnOnHurt()
    {
        if (behaviourStateMachine.CurrentStateType == EnemyStateEnum.Dead)
            return;

        SetActionState(EnemyActionStateEnum.Chase);
    }
    public virtual void OnPauseCall(bool flag =false)
    {
   
        flag = TimeManager.Instance.TimePaused;


        enemyAnimatorScript.PauseAnimator(flag);
    }
    public virtual void OnDamage(float damage)
    {
        if (enemyHealth.IsDead || TimeManager.Instance.TimePaused)
            return;
        
        enemyHealth.DamageHealth(damage);

        if (enemyHealth.IsDead)
        {
            behaviourStateMachine.SetState(EnemyStateEnum.Dead);
        }
        else
            SetActionState(EnemyActionStateEnum.Hurt);
    }
    public void SetActionState(EnemyActionStateEnum actionState) => actionStateMachine.SetState(actionState);
    public void CallInitialAttack() => enemyAnimatorScript.TriggerInitialAttack();
    public void CallSecondAttack() => enemyAnimatorScript.TriggerSecondAttack();
    public void CallHurt() => enemyAnimatorScript.TriggerHurt();
    public void CallDeath() => enemyAnimatorScript.TriggerDeath();

    public void ExecuteAttack()
    {
        switch (attackType)
        {
            case 0: CallInitialAttack(); break;
            case 1: CallSecondAttack(); break; 
            default: CallInitialAttack(); break;
        }
    }
    #endregion

    #region Debug Methods

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.right *-1, mainStats.DetectionRange);
        Gizmos.DrawWireCube(AttackPoint.position, AttackSize);
    }
    #endregion
}

[System.Serializable]
public enum EnemyTypeEnum
{
    Slime,
    Goblin,
    Mushroom,
    Skeleton,
    FlyingEye
}

[System.Serializable]
public class EnemyStats
{
    public float MaxHealth;
    public float MovementSpeed;
    public float AttackSpeed;
    public List<float> DamageData;
    public float StopTime;
    public Vector2 DetectionRange;
    public List<Vector2> AttackRange;
}