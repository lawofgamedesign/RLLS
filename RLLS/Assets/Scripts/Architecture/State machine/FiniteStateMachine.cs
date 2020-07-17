using System;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<TContext>
{

    //refernce to the object whose state the machine is handling
    private readonly TContext _context;


    //cache of state machine's states
    private readonly Dictionary<Type, State> _stateCache = new Dictionary<Type, State>();


    //public access to the current state
    public State CurrentState { get; private set; }


    //state that will be transitioned to; such states are store separately, so as to avoid changing the current state in the middle of a frame
    private State _pendingState;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context">The type of the object whose state this state machine manages</param>
    public FiniteStateMachine(TContext context)
    {
        _context = context;
    }


    public void Tick()
    {
        //handle pending transition if TransitionTo has already been called directly outside the state machine (ill-advised but possible)
        PerformPendingTransition();


        //confirm that there is a current state
        Debug.Assert(CurrentState != null, "No current state. Did you forget to transition to an initial state?");


        //run the current state
        CurrentState.Tick();


        //handle any transition started during the frame
        PerformPendingTransition();
    }


    /// <summary>
    /// Queue transition to a new state.
    /// </summary>
    /// <typeparam name="TState">The state to be transitioned to</typeparam>
    public void TransitionTo<TState>() where TState : State
    {
        _pendingState = GetOrCreateState<TState>();
    }


    private TState GetOrCreateState<TState>() where TState : State
    {
        State state;

        if (_stateCache.TryGetValue(typeof(TState), out state))
        {
            return (TState) state;
        } else
        {
            var newState = Activator.CreateInstance<TState>();
            newState.Parent = this;
            newState.Init();
            _stateCache[typeof(TState)] = newState;
            return newState;
        }
    }


    private void PerformPendingTransition()
    {
        if (_pendingState != null)
        {
            if (CurrentState != null) CurrentState.OnExit();
            CurrentState = _pendingState;
            CurrentState.OnEnter();
            _pendingState = null;
        }
    }


    public abstract class State
    {
        //the state machine for this state; allows the state to reach out to the machine and request transitions to other states
        internal FiniteStateMachine<TContext> Parent { get; set; }


        //easy access to the context object
        protected TContext Context { get { return Parent._context; } }


        /// <summary>
        /// Easy way to ask the state machine to transition to another state.
        /// </summary>
        /// <typeparam name="TState">The type of the state to transition to</typeparam>
        protected void TransitionTo<TState>() where TState : State
        {
            Parent.TransitionTo<TState>();
        }


        /// <summary>
        /// Called once when state created.
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// Called when state becomes active.
        /// </summary>
        public virtual void OnEnter() { }



        /// <summary>
        /// Called when state becomes inactive.
        /// </summary>
        public virtual void OnExit() { }


        /// <summary>
        /// Called each frame while state active.
        /// </summary>
        public virtual void Tick() { }


        /// <summary>
        /// Called when state machine is cleared.
        /// </summary>
        public virtual void CleanUp() { }
    }
}
