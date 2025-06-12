using System;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Run,
    Attack
}



public interface IState<TOwner, TState> where TOwner : MonoBehaviour where TState : Enum
{
    void OnEnter(TOwner owner);
    void OnUpdate(TOwner owner);
    void OnFixedUpdate(TOwner owner);
    void OnExit(TOwner entity);

    TState CheckTransition(TOwner owner);
}

public class StateMachine<T, TState> where T : MonoBehaviour where TState : Enum
{
    private T ownerEntity;
    private IState<T, TState> currentState;

    public void Setup(T owner, IState<T, TState> entryState)
    {
        ownerEntity = owner;
        ChangeState(entryState);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate(ownerEntity);
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate(ownerEntity);
        }
    }

    public void ChangeState(IState<T, TState> newState)
    {
        if (newState == null)
            return;


        if (currentState != null)
        {
            currentState.OnExit(ownerEntity);
        }

        currentState = newState;
        currentState.OnEnter(ownerEntity);
    }
}