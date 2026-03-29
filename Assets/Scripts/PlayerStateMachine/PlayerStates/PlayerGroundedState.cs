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
<<<<<<< HEAD
        _playerMovement.playerStateText.SetText("player State: Grounded");
=======
        _playerMovement.StateText.text = "Player State: Grounded State";
        if (_playerMovement.velocity.y < 0) {
            _playerMovement.velocity.y = _playerMovement.playerConfig.constGravity; //reset the vertical velocity to 0 when we land on the ground to prevent the player from sticking to the ground or bouncing up and down when landing.
        }
>>>>>>> bcdbe6d9a97aeb55bd7a9936617f606852fc5bd2
    }

    public void Tick() {

        _playerMovement.HandleMovement(_playerMovement.playerConfig.moveSpeed);

        if (!_playerMovement.isGrounded) {
            _stateMachine.ChangeState(_playerStateMachine.fallingState);
            return;
        }
        if (_playerMovement.moveInput == Vector2.zero)
        {
            _playerMovement.StateText.text = "Player State: Grounded State";
        }
        else if (_playerMovement.isSprinting) {
            _stateMachine.ChangeState(_playerStateMachine.sprintingState);
        }
        else
        {
            _stateMachine.ChangeState(_playerStateMachine.walkingState);
        }
    }
    public void Exit() {
        //Debug.Log("Exited Grounded State");
    }
}
