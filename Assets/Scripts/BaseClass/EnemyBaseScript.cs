using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    #region Variables
    private EnemyStateMachine behaviourStateMachine;
    private EnemyActionStateMachine actionStateMachine;
    [SerializeField]
    private EnemyStats mainStats;

    
    public List<GameObject> MovementList;
    public EnemyTypeEnum EnemyType;
    public GameObject EnemySpriteObj;


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



    #endregion

    #endregion

    #region Base Methods
    private void Awake()
    {
        behaviourStateMachine = GetComponent<EnemyStateMachine>();
        actionStateMachine = GetComponent<EnemyActionStateMachine>();
    }


    // Start is called before the first frame update
   public virtual void Start()
    {
        
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

        if (Vector3.Distance(EnemySpriteObj.transform.position, MovementList[movementCounter].transform.position) <= 0.1f)
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

        }
        else
        {
            EnemySpriteObj.transform.position = Vector3.MoveTowards(EnemySpriteObj.transform.position, MovementList[movementCounter].transform.position, mainStats.MovementSpeed * TimeManager.Instance.DeltaTime);
        }
        
    }

    public virtual void DetectionFunction()
    {
        if (detectionTimer > 0.1f)
        {
            detectionTimer = 0f;

            Collider2D ColliderHit = Physics2D.OverlapBox(EnemySpriteObj.transform.position + EnemySpriteObj.transform.right * -1, mainStats.DetectionRange, 0, DetectionLayerMask);
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
            if (Vector3.Distance(EnemySpriteObj.transform.position, TargetObj.transform.position) > mainStats.AttackRange[0].x)
            {
                Vector3 newDest = Vector3.MoveTowards(EnemySpriteObj.transform.position, TargetObj.transform.position, mainStats.MovementSpeed * TimeManager.Instance.DeltaTime);
                newDest.y = EnemySpriteObj.transform.position.y;
                EnemySpriteObj.transform.position = newDest;
            }
        }
    }

    public virtual void CheckForAttack()
    {
        if (attackTimer <= 0f)
        {
            if (TargetObj != null)
            {
                float range = Vector3.Distance(EnemySpriteObj.transform.position, TargetObj.transform.position);
                for (int i = 0; i < mainStats.AttackRange.Count; i++)
                {
                    if (range >= mainStats.AttackRange[i].x && range <= mainStats.AttackRange[i].y)
                    {
                        /*actionStateMachine.SetState(EnemyActionStateEnum.Attack);
                        attackType = i; break;*/

                        Debug.Log("I'mma attack");
                    }
                }
            }
        }
    }

    public void SetActionState(EnemyActionStateEnum actionState) => actionStateMachine.SetState(actionState);
    #endregion

    #region Debug Methods

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(EnemySpriteObj.transform.position + EnemySpriteObj.transform.right *-1, mainStats.DetectionRange);
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
    public float MovementSpeed;
    public float AttackSpeed;
    public List<float> DamageData;
    public float StopTime;
    public Vector2 DetectionRange;
    public List<Vector2> AttackRange;
}