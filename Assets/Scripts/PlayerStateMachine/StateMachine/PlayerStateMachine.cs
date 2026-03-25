using UnityEngine;

public sealed class PlayerStateMachine : MonoBehaviour
{
    [Header("State Machine")]
    private StateMachine _stateMachine;

    [Header("Player States")]
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerAirborneState fallingState { get; private set; }
    public PlayerSprintingState sprintingState { get; private set; }
    public PlayerWalkingState walkingState { get; private set; }

    [Header("Player Controller")]
    private PlayerMovement _playerMovement;

    public void Awake()
    {
        _stateMachine = new StateMachine();
        _playerMovement = GetComponent<PlayerMovement>();
        groundedState = new PlayerGroundedState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        fallingState = new PlayerAirborneState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        sprintingState = new PlayerSprintingState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        walkingState = new PlayerWalkingState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
    }

    public void Start()
    {
        //TODO: Change to the deafult state when game starts
        _stateMachine.ChangeState(groundedState); 
    }
    public void Update()
    {
        _playerMovement.GroundCheck();
        _stateMachine.Tick(); //Runs the currentState every frame
    }
}

