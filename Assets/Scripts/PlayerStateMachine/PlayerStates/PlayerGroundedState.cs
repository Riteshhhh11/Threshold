using UnityEngine;

public sealed class PlayerGroundedState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerStateMachine _playerStateMachine;


    public PlayerGroundedState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine)
    {
        _stateMachine = sm;
        _playerMovement = player;
        _playerStateMachine = playerStateMachine;
    }

    public void Enter() {
        _playerMovement.playerStateText.SetText("player State: Grounded");
    }

    public void Tick() {

        _playerMovement.HandleMovement(_playerMovement.playerConfig.moveSpeed);

        if (!_playerMovement.isGrounded) {
            _stateMachine.ChangeState(_playerStateMachine.fallingState);
        }
        else if (_playerMovement.moveInput != Vector2.zero)
        {
            if (_playerMovement.isSprinting) {
                _stateMachine.ChangeState(_playerStateMachine.sprintingState);
            }
            else
            {
                _stateMachine.ChangeState(_playerStateMachine.walkingState);
            }
        }
    }
    public void Exit() {
        //Debug.Log("Exited Grounded State");
    }
}
