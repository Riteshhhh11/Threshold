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
    public PlayerInteractionState interactionState { get; private set; }

    [Header("Player Controller")]
    private PlayerMovement _playerMovement;
    private PlayerCamera _playerCamera;

    public void Awake()
    {
        _stateMachine = new StateMachine();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCamera = GetComponentInChildren<PlayerCamera>();
        groundedState = new PlayerGroundedState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        fallingState = new PlayerAirborneState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        sprintingState = new PlayerSprintingState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        walkingState = new PlayerWalkingState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this);
        interactionState = new PlayerInteractionState(sm: _stateMachine, player: _playerMovement, playerStateMachine: this, camera: _playerCamera);
    }

    public void Start()  
    {
        //TODO: Change to the deafult state when game starts
        _stateMachine.ChangeState(groundedState); 
    }
    public void Update()
    {
        _stateMachine.Tick(); //Runs the currentState every frame
    }

    //Method so the external scripts can change the state of the player state machine
    public void ChangeState(IState newState) {
        _stateMachine.ChangeState(newState);
    }
}

