using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionStateMachine : StateMachine<EnemyActionBaseState, EnemyActionStateEnum> //State Machine
{
    #region Variables

    //[SerializeField] BaseClass baseClass;

    #endregion

    #region Base Functions
    private void Awake()
    {
        // Sets up Essential components
       
        //baseClass = this.GetComponentInParent<BaseClass>();

        //Array of States to be initialized for StateMachine to be set to
        states = new EnemyActionBaseState[4];
        states[0] = new EnemyIdleActionState(this);
        states[1] = new EnemyAttackActionState(this);
        states[2] = new EnemyBlockActionState(this);
        states[2] = new EnemyHurtActionState(this);



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
    //private BaseClass baseClass

    // CONSTRUCTOR
    protected EnemyActionBaseState(EnemyActionStateMachine stateMachine /* ANY OTHER PARAMETERS */)
    {
        
        this.StateMachine = stateMachine;
       
    }

    // FUNCTIONS

   
    protected void SetState(EnemyActionStateEnum state)
    {

        StateMachine.SetState(state);
    }
    


}