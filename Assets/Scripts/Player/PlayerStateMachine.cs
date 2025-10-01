using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }

    public void Initialize(IState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Execute();
    }
}