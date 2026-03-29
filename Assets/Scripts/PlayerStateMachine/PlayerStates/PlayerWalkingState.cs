using UnityEngine;

public sealed class PlayerWalkingState: IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerStateMachine _playerStateMachine;


    public PlayerWalkingState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine)
    {
        _stateMachine = sm;
        _playerMovement = player;
        _playerStateMachine = playerStateMachine;
    }

    public void Enter() {
        _playerMovement.playerStateText.SetText("player State: Walking");
    }
    public void Tick() {
        float walkingSpeed = _playerMovement.playerConfig.moveSpeed;
        _playerMovement.HandleMovement(walkingSpeed);

        if (!_playerMovement.isGrounded)
        {
            _stateMachine.ChangeState(_playerStateMachine.fallingState);
        }
        else if (_playerMovement.moveInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_playerStateMachine.groundedState);
        }
        else if (_playerMovement.isSprinting) {
            _stateMachine.ChangeState(_playerStateMachine.sprintingState);  
        }
    }
    public void Exit() {
        //Debug.Log("Exited Walking State");
    }
}
