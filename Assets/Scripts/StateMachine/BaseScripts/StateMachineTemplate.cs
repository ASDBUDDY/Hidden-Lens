using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineTemplate : StateMachine<BaseStateTemplate, StateEnum> //State Machine
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
        states = new BaseStateTemplate[3];
        states[0] = new StateTemplate(this);
        states[1] = new StateTemplate(this);
        states[2] = new StateTemplate(this);



        //Set to Default State Post Initialisation
        SetState(StateEnum.Default);
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
public enum StateEnum
{
    Default

}

/// <summary>
/// State Base Class
/// </summary>
public abstract class BaseStateTemplate : State<StateEnum>
{
    // VARIABLES

    private StateMachineTemplate StateMachine;
    //private BaseClass baseClass

    // CONSTRUCTOR
    protected BaseStateTemplate(StateMachineTemplate stateMachine /* ANY OTHER PARAMETERS */)
    {
        
        this.StateMachine = stateMachine;
       
    }

    // FUNCTIONS

   
    protected void SetState(StateEnum state)
    {

        StateMachine.SetState(state);
    }
    


}