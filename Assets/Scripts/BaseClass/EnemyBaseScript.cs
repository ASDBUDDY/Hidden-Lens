using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    #region Variables
    protected EnemyStateMachine behaviourStateMachine;
    protected EnemyActionStateMachine actionStateMachine;
    protected EnemyAnimatorScript enemyAnimatorScript;
    protected SpriteRenderer enemySprite;
    [SerializeField]
    protected EnemyStats mainStats;
    protected HealthComponent enemyHealth;
    [SerializeField]
    protected Material enemyAuraMaterial;
    [SerializeField]
    protected Material enemyLightMaterial;
    
    public List<GameObject> MovementList;
    public EnemyTypeEnum EnemyType;
    public List<GameObject> ImpactFX;
    public GameObject DeathFX;


    #region Movement Variables
    protected float stopTimer = 0f;
    protected int movementCounter = 0;


    #endregion

    #region Detection Variables
    public LayerMask DetectionLayerMask;
    protected float detectionTimer = 0f;

    protected GameObject TargetObj;

    #endregion

    #region Attack Variables
    protected float attackTimer = 0f;
    protected int attackType = 0;
    public Transform AttackPoint;
    public Vector2 AttackSize = Vector2.one;

    [Header("For DEBUG")]
    [SerializeField]
    string CurrentBehaviourState = "";
    [SerializeField]
    string CurrentActionState = "";
    #endregion

    #endregion

    #region Base Methods
    public virtual void Awake()
    {
        behaviourStateMachine = GetComponent<EnemyStateMachine>();
        actionStateMachine = GetComponent<EnemyActionStateMachine>();
        enemyAnimatorScript = GetComponent<EnemyAnimatorScript>();
        enemySprite = GetComponent<SpriteRenderer>();
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
        StateUpdate();
    }
    #endregion

    #region Functionality Body

    public bool IsDead => enemyHealth.IsDead;
    public virtual void MovementFunction()
    {
        if(movementCounter >= MovementList.Count)
            movementCounter = 0;

        if ( Mathf.Abs(transform.position.x - MovementList[movementCounter].transform.position.x) <= 0.1f)
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

            MovementPos.y = transform.position.y;

                transform.position = MovementPos;
        }
        
    }
    protected void SetRotationAndVelocity(float diffPosition)
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

            Collider2D ColliderHit = Physics2D.OverlapBox(transform.position + transform.right * -1f, mainStats.DetectionRange, 0, DetectionLayerMask);
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
            if (Mathf.Abs(transform.position.x - TargetObj.transform.position.x) > mainStats.AttackRange[0].x)
            {
                Vector3 newDest = Vector3.MoveTowards(transform.position, TargetObj.transform.position, mainStats.MovementSpeed * 1.1f * TimeManager.Instance.DeltaTime);

                float diffPosition = transform.position.x - newDest.x;

                SetRotationAndVelocity(diffPosition);
                newDest.y = transform.position.y;

                transform.position = newDest;
            }
        }
    }

    
    protected IEnumerator OnDeath()
    {
        yield return new CustomWaitForSeconds(2f);

        this.gameObject.SetActive(false);
    }
    public virtual void CheckForAttack()
    {
        if (attackTimer <= 0f && actionStateMachine.CurrentStateType != EnemyActionStateEnum.Attack && actionStateMachine.CurrentStateType !=EnemyActionStateEnum.Hurt)
        {
            if (TargetObj != null)
            {
                float range = Mathf.Abs(transform.position.x - TargetObj.transform.position.x);
                for (int i = 0; i < mainStats.AttackRange.Count; i++)
                {
                    if (range >= mainStats.AttackRange[i].x && range <= mainStats.AttackRange[i].y)
                    {
                        actionStateMachine.SetState(EnemyActionStateEnum.Attack);
                        attackType = i;
                        
                       
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

        Invoke(nameof(ReturnToChase), 1f);
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
        if (enemyHealth.IsDead || TimeManager.Instance.TimePaused || actionStateMachine.CurrentStateType == EnemyActionStateEnum.Block)
            return;
        
        enemyHealth.DamageHealth(damage);

        PlayImpact();

        if (enemyHealth.IsDead)
        {
            behaviourStateMachine.SetState(EnemyStateEnum.Dead);
            DeathFX.SetActive(true);
        }
        else
            SetActionState(EnemyActionStateEnum.Hurt);
    }
    protected void ReturnToChase() => SetActionState(EnemyActionStateEnum.Chase);
    public void SetActionState(EnemyActionStateEnum actionState) => actionStateMachine.SetState(actionState);
    public void CallInitialAttack() => enemyAnimatorScript.TriggerInitialAttack();
    public void CallSecondAttack() => enemyAnimatorScript.TriggerSecondAttack();
    public void CallHurt() => enemyAnimatorScript.TriggerHurt();
    public void CallDeath() 
    {
        CancelInvoke(nameof(ReturnToChase));
        enemyAnimatorScript.TriggerDeath(); 
        StartCoroutine(OnDeath()); 
    }

    public virtual void ExecuteAttack()
    {
        switch (attackType)
        {
            case 0: CallInitialAttack(); break;
            case 1: CallSecondAttack(); break; 
            default: CallInitialAttack(); break;
        }

        enemyAnimatorScript.SetVelocity(0f);
    }
    public virtual void ToggleBlock(bool flag = true)
    {

    }

    public void SwapMaterial(bool flag = false)
    {
        enemySprite.material = flag ? enemyLightMaterial : enemyAuraMaterial;
    }


    public void PlayImpact()
    {
        foreach(var item in ImpactFX)
        {
            if (item.activeInHierarchy)
                continue;

            item.SetActive(true);
            break;
        }
    }
    #endregion

    #region Debug Methods

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.right*-1f, mainStats.DetectionRange);
        Gizmos.DrawWireCube(AttackPoint.position, AttackSize);
    }

    private void StateUpdate()
    {
        CurrentBehaviourState = behaviourStateMachine.CurrentStateType.ToString();
        CurrentActionState = actionStateMachine.CurrentStateType.ToString();
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