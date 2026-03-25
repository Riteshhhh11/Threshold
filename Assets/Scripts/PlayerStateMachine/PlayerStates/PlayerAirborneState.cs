using UnityEngine;

public sealed class PlayerAirborneState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly PlayerMovement _player; //Data will be borrowed from the PlayerMovement script to be used in this state
    private readonly PlayerStateMachine _playerStateMachine;

    public PlayerAirborneState(StateMachine sm, PlayerMovement player, PlayerStateMachine playerStateMachine)
    {
        _stateMachine = sm;
        _player = player;
        _playerStateMachine = playerStateMachine;
    }
    public void Enter() {
        _player.StateText.text = "Player State: Falling State";
    }

    public void Tick() { 
        //Falling needs gravity to be applied in every frame so the player can fall down.
        _player.ApplyGravity();
        //Apply AirControl when the player is falling so the player can control their movement in the air.
        float airSpeed = _player.playerConfig.moveSpeed * _player.playerConfig.airControlFactor;
        _player.HandleMovement(airSpeed);
        //Transition to the landing state when the player is grounded again.
        if (_player.isGrounded)
        {
            //// We just landed this frame → decide next grounded state
            //if (_player.moveInput == Vector2.zero)
            //{
            //    _stateMachine.ChangeState(_playerStateMachine.groundedState);
            //}
            //else if (_player.isSprinting)
            //{
            //    _stateMachine.ChangeState(_playerStateMachine.sprintingState);
            //}
            //else
            //{
            //    _stateMachine.ChangeState(_playerStateMachine.walkingState);
            //}
            if (_player.isGrounded) {
                _stateMachine.ChangeState(_playerStateMachine.groundedState);
            }
        }
    }

    public void Exit() {
        //Debug.Log("Exited Falling State");
    }
}
