using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class InteractionState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerCamera _playerCamera;


    public InteractionState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine, PlayerCamera playerCamera)
    {
        _playerMovement = player;
        _stateMachine = sm;
        _playerStateMachine = playerStateMachine;
        _playerCamera = playerCamera;
    }
    public void Enter() {
        _playerMovement.DisabledInput();
        Debug.Log("Interaction State Enter Function Called");
        _playerCamera.DisableCamera();
        CursorUtility.UnlockAndShowCursor();
    }

    public void Tick() {
    
    }

    public void Exit() {
        _playerMovement.EnableInput();
        Debug.Log("Interaction State Exit Function Called");
        _playerCamera.EnableCamera();
        CursorUtility.LockAndHideCursor();
        Debug.Log("Exited Interaction State");
    }
}
