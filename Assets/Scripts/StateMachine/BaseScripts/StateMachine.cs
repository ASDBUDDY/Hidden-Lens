using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


    [Serializable]
    public abstract class StateMachine<T1, T2> : MonoBehaviour //Abstract Class of StateMachine with Generic Type T1 and T2
        where T1 : State<T2> //Where Generic Type T1 is a State class of Type T2
        where T2 : Enum// Where T2 is a type of Enum also subscribed to T1/State Class
    {
        /// <summary>
        /// Array of states, to be initialized on awake.
        /// </summary>
        protected T1[] states;
        /// <summary>
        /// Getter to access the Current State type of Enum.
        /// </summary>
        [SerializeField]public T2 CurrentStateType => CurrentState.Type;
        /// <summary>
        /// Getter to access the Current State of the State Machine.
        /// </summary>
        public T1 CurrentState { get; protected set; }
        public T1 TempState { get; protected set; }

        /// <summary>
        /// Returns an object of the given state type.
        /// </summary>
        /// <param name="toGetState"></param>
        /// <returns>State object in the state machine of the the given type.</returns>
        protected T1 GetState(T2 toGetState)
        {
            if (ReferenceEquals(states, null))
                return null;
            
            foreach (var state in states)
                if (state.Type.Equals(toGetState))
                    return state;

            Debug.LogError($"{name} state machine doesn't have state with type {toGetState.ToString()}");
            return null;
        }

        /// <summary>
        /// Sets a state to the given state.
        /// </summary>
        /// <param name="nextStateType">The state to which the state machine should transition to.</param>
        public void SetState(T2 nextStateType)
        {
             TempState = GetState(nextStateType);
            if (!TempState)
                return;
        if (TempState == CurrentState)
                return;

            CurrentState?.OnExit();
            TempState.OnEnter();

            CurrentState = TempState;
        }

      


    }
