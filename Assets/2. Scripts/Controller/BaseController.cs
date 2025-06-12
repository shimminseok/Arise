using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseController<TController, TState> : MonoBehaviour where TController : BaseController<TController, TState>
    where TState : Enum

{
    public StatManager         StatManager         { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }
    public NavMeshAgent        Agent               { get; private set; }

    private StateMachine<TController, TState> stateMachine;
    private IState<TController, TState>[] states;
    protected TState CurrentState;

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        Agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine<TController, TState>();
    }

    protected virtual void Start()
    {
        SetupState();
    }

    protected virtual void Update()
    {
        TryStateTransition();
        stateMachine?.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    private void SetupState()
    {
        Array values = Enum.GetValues(typeof(TState));
        states = new IState<TController, TState>[values.Length];
        for (int i = 0; i < states.Length; i++)
        {
            TState state = (TState)values.GetValue(i);
            states[i] = GetState(state);
        }

        CurrentState = (TState)values.GetValue(0);
        stateMachine.Setup((TController)this, states[0]);
    }


    protected void ChangeState(TState newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = newState;
    }

    private void TryStateTransition()
    {
        int currentIndex = Convert.ToInt32(CurrentState);
        var next         = states[currentIndex].CheckTransition((TController)this);
        if (!next.Equals(CurrentState))
        {
            ChangeState(next);
        }
    }

    protected abstract IState<TController, TState> GetState(TState state);

    public abstract void FindTarget();
    public abstract void Movement();
}