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
<<<<<<< HEAD
        _playerMovement.playerStateText.SetText("player State: Walking");
=======
        _playerMovement.StateText.text = "Player State: Walking State";
>>>>>>> bcdbe6d9a97aeb55bd7a9936617f606852fc5bd2
    }
    public void Tick() {
        float walkingSpeed = _playerMovement.playerConfig.moveSpeed;
        _playerMovement.HandleMovement(walkingSpeed);

        if (_playerMovement.moveInput == Vector2.zero)
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
