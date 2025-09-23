using System;
using UnityEngine;


    [Serializable]
    public abstract class State<T>// Abstract Class of State with Generic Type T
        where T : Enum //Where Generic T is of type Enum
    {
    /// <summary>
    /// Getter for Type assigned to this specific State Class from the Enum T
    /// </summary>
        public abstract T Type { get; }

    /// <summary>
    /// Execution for when this State is first Set
    /// </summary>
        internal virtual void OnEnter()
        {
        }

    /// <summary>
    /// Execution for when another State is set after this State
    /// </summary>
        internal virtual void OnExit()
        {
        }
    
    /// <summary>
    /// Execution for Update for the lifetime of this State
    /// </summary>
    internal virtual void Update()
    {

    }
    /// <summary>
    /// Execution for Fixed Update for the lifetime of this State
    /// </summary>
    internal virtual void FixedUpdate()
    {

    }

    /// <summary>
    /// For Null Check Reference of the State
    /// </summary>
    /// <param name="state"></param>
        public static implicit operator bool(State<T> state)
        {
            return !ReferenceEquals(state, null);
        }
    }
