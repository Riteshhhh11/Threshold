using UnityEngine;

public class PlayerInteractionState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _player; //Data will be borrowed from the PlayerMovement script to be used in this state
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerCamera _camera;

    public PlayerInteractionState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine, PlayerCamera camera)
    {
        _stateMachine = sm;
        _player = player;
        _playerStateMachine = playerStateMachine;
        _camera = camera;
    }
    public void Enter() {
        Debug.Log("Entered Interaction State");
        _player.DisableMovement();
        _camera.DisableCamera();
        CursorUtility.UnlockAndShowCursor();
    }

    public void Tick() {
    
    }

    public void Exit() {
        Debug.Log("Exited Interaction State");
        _player.EnableMovement();
        _camera.EnableCamera();
        CursorUtility.LockAndHideCursor();
    }
}
