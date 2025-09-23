using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<EnemyBaseState, EnemyStateEnum> //State Machine
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
        states = new EnemyBaseState[3];
        states[0] = new EnemyIdleState(this);
        states[1] = new EnemyAggroState(this);
        states[2] = new EnemyDeadState(this);



        //Set to Default State Post Initialisation
        SetState(EnemyStateEnum.Idle);
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
public enum EnemyStateEnum
{
    Idle,
    Aggro,
    Dead

}

/// <summary>
/// State Base Class
/// </summary>
public abstract class EnemyBaseState : State<EnemyStateEnum>
{
    // VARIABLES

    private EnemyStateMachine StateMachine;
    //private BaseClass baseClass

    // CONSTRUCTOR
    protected EnemyBaseState(EnemyStateMachine stateMachine /* ANY OTHER PARAMETERS */)
    {
        
        this.StateMachine = stateMachine;
       
    }

    // FUNCTIONS

   
    protected void SetState(EnemyStateEnum state)
    {

        StateMachine.SetState(state);
    }
    


}