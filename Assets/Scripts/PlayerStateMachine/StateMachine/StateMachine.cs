using UnityEngine;

public sealed class StateMachine
{
    private IState _currentState;

    public void ChangeState(IState nextState) {
        if (nextState == null) {
            return;
        }
        if (ReferenceEquals(_currentState, nextState)) {
            return;
        }
        _currentState?.Exit();
        _currentState = nextState;
        _currentState?.Enter();
    }

    public void Tick() {
        _currentState?.Tick();
    }
}
