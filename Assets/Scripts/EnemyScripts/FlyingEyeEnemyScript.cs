using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FlyingEyeEnemyScript : EnemyBaseScript
{
    [Header("FlyingEye Specific")]

 
    private BoxCollider2D enemyCollider;
    private Vector2 colliderOffset = new Vector2(-0.05655462f, -0.000712961f);
    private Vector2 blockColliderOffset = new Vector2(0.1f, -0.000712961f);

    [SerializeField]
    private float dashSpeed = 5f;
    public Transform AttackPoint2;
    public Vector2 AttackSize2;
    private Coroutine DashAttack;

    public override void Awake()
    {
        base.Awake();
       enemyCollider = GetComponent<BoxCollider2D>();
      
    }
    // Start is called before the first frame update
    public override void Start()
    {
        EnemyType = EnemyTypeEnum.FlyingEye;
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
            case 1:
                attackPos = AttackPoint.position;
                attackSize = AttackSize2;
                if (attackTimer > 0f)
                    return;
                break;
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

        if(attackType == 1)
        {
            CancelInvoke(nameof(ReturnToChase));
        }

        Invoke(nameof(ReturnToChase), 1f);

    }

   
    private IEnumerator DashRoutine(Vector3 pos)
    {
        yield return new CustomWaitForSeconds(0.1f);

        while(actionStateMachine.CurrentStateType == EnemyActionStateEnum.Attack && Mathf.Abs(transform.position.x - pos.x)>0.1f)
        {
            Vector3 newDest = Vector3.MoveTowards(transform.position, pos, dashSpeed * TimeManager.Instance.DeltaTime);

            float diffPosition = transform.position.x - newDest.x;

            SetRotationAndVelocity(diffPosition);
            newDest.y = transform.position.y;

            transform.position = newDest;

            yield return new WaitForEndOfFrame();
        }
    }

    public override void ExecuteAttack()
    {
        base.ExecuteAttack();

        if (attackType == 1)
        {
           if(DashAttack != null)
           {
                StopCoroutine(DashAttack);
           }
          DashAttack= StartCoroutine(DashRoutine(AttackPoint2.position));
        }
    }
    
    
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireCube(AttackPoint2.position, AttackSize2);
    }
}
