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

    #endregion

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
    }

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

}