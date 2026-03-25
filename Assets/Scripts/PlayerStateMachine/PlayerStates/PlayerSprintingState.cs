using System;
using UnityEngine;

public sealed class PlayerSprintingState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerStateMachine _playerStateMachine;

    public PlayerSprintingState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine)
    {
        _stateMachine = sm;
        _playerMovement = player;
        _playerStateMachine = playerStateMachine;
    }

    public void Enter() {
        _playerMovement.StateText.text = "Player State: Sprinting State";
    }

    public void Tick() {
        float sprintSpeed = _playerMovement.playerConfig.sprintSpeed;
        _playerMovement.HandleMovement(sprintSpeed);
        if (_playerMovement.moveInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_playerStateMachine.groundedState);
        }
        else if (!_playerMovement.isSprinting) {
            _stateMachine.ChangeState(_playerStateMachine.walkingState);    
        }
    }

    public void Exit() {  

    }
}
