using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyActionStateMachine : StateMachine<EnemyActionBaseState, EnemyActionStateEnum> //State Machine
{
    #region Variables

    [SerializeField] EnemyBaseScript baseClass;

    #endregion

    #region Base Functions
    private void Awake()
    {
        // Sets up Essential components
       
        baseClass = this.GetComponent<EnemyBaseScript>();

        //Array of States to be initialized for StateMachine to be set to
        states = new EnemyActionBaseState[5];
        states[0] = new EnemyIdleActionState(this,baseClass);
        states[1] = new EnemyAttackActionState(this, baseClass);
        states[2] = new EnemyBlockActionState(this, baseClass);
        states[3] = new EnemyHurtActionState(this, baseClass);
        states[4] = new EnemyChaseActionState(this, baseClass);



        //Set to Default State Post Initialisation
        SetState(EnemyActionStateEnum.Idle);
    }

    

    /// <summary>
    /// Calling Update of State if it Exists
    /// </summary>
    public void CallStateUpdate()
    {
        CurrentState?.Update();

    }
    /// <summary>
    /// Calling Fixed Update of State if it Exists
    /// </summary>
    public void CallStateFixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
    #endregion

    #region Functions

    //Some Functions(){}

    #endregion
}
/// <summary>
/// Enum of States for  StateMachine
/// </summary>
public enum EnemyActionStateEnum
{
    Idle,
    Chase,
    Attack,
    Block,
    Hurt

}

/// <summary>
/// State Base Class
/// </summary>
public abstract class EnemyActionBaseState : State<EnemyActionStateEnum>
{
    // VARIABLES

    private EnemyActionStateMachine StateMachine;
    private EnemyBaseScript BaseClass;

    // CONSTRUCTOR
    protected EnemyActionBaseState(EnemyActionStateMachine stateMachine, EnemyBaseScript enemyBase /* ANY OTHER PARAMETERS */)
    {
        
        this.StateMachine = stateMachine;
        this.BaseClass = enemyBase;
       
    }

    // FUNCTIONS

   
    protected void SetState(EnemyActionStateEnum state)
    {

        StateMachine.SetState(state);
    }
    

    protected void ChaseFunction() => BaseClass.ChaseFunction();

}