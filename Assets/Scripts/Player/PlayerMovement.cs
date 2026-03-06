using Unity.VisualScripting;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Assets")]
    [SerializeField] private CharacterController PlayerController;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] public MovementConfig playerConfig;

    [Header("Ground Check")]
    [SerializeField] private Transform groundSpherePosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public bool isGrounded;

    [Header("Ceiling Check")]
    [SerializeField] private Transform ceilingSpherePosition;
    [SerializeField] private LayerMask ceilingLayer;
    [SerializeField] public bool isCeilinged;

    [Header("Player Inputs")]
    [SerializeField] public Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] public bool isSprinting;
    [SerializeField] private Vector3 horizontalVelocity; //will hold current smooth horizontal velocity
    [SerializeField] private Vector3 horizontalVelocityRef; //will hold the current vertical velocity 

    private void Awake()
    {
        PlayerController = GetComponent<CharacterController>();
        playerControls = new PlayerControls(); // Initialize the PlayerControls 
        horizontalVelocity = Vector3.zero;
        CursorUtility.LockAndHideCursor();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        playerControls.Player.Jump.performed += OnJump;
        playerControls.Player.Sprint.performed += OnSprint;
        playerControls.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Player.Jump.performed -= OnJump;
        playerControls.Player.Sprint.performed -= OnSprint;
        playerControls.Player.Sprint.canceled -= OnSprint;
        playerControls.Disable();
    }
    private void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        playerConfig.jumpBufferTimer = playerConfig.jumpBufferTime; 
    }
    private void OnSprint(InputAction.CallbackContext context) {
        bool newValue = context.ReadValueAsButton();
        isSprinting = newValue;
    }
    public void Update()
    {
        GroundCheck();
        CeilingCheck();
        HandleJump();
    }
    private void LateUpdate() //IDK why but this fixed 99% of the jitter effect on the objects.
    {
        PlayerController.Move(velocity * Time.deltaTime);
    }
    private void FixedUpdate() //for physics calculation on a fixed timestep.
    {
        ApplyGravity();
    }
    public void HandleMovement(float desiredSpeed) {
        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y); 
        inputDirection = transform.TransformDirection(inputDirection);
        Vector3 targetHorizontalVelocity = inputDirection * desiredSpeed; //storing the scaled speed from the input into targetHorizontalVelocity (from 0 to 5, 5 is the walking speed).
        //Debug.Log($"Target horizontal velocity before smoothing: {targetHorizontalVelocity}");
        float smoothTime = isGrounded ? playerConfig.smoothGroundMovement : playerConfig.smoothAirMovement; //decides the smoothing factor based on the player's state.
        //Parameters explained:
        //   1. current value          → horizontalVelocity (where we are now)
        //   2. target value           → targetHorizontalVelocity (where we want to go)
        //   3. ref currentVelocity    → horizontalVelRef (SmoothDamp updates this internally to calculate damping) always needs a ref keyword and only accepts a reference.
        //   4. smoothTime             → time in seconds to reach ~63% of the way to target (feels natural)
        horizontalVelocity = Vector3.SmoothDamp(
                horizontalVelocity, //current horizontal velovity of the player
                targetHorizontalVelocity, //target velocity we want to reach based on input and speed
                ref horizontalVelocityRef, //ref keyword passes the variable by reference.
                smoothTime //smoothing factor that determines the time it takes to reach the target velocity.
            );
        //Debug.Log($"Smoothed horizontal velocity after vector3.SmoothDamp {horizontalVelocity}");
        velocity.x = horizontalVelocity.x; 
        velocity.z = horizontalVelocity.z;
        //Debug.Log($"smooth velocity on x: {velocity.x}");
        //Debug.Log($"smooth velocity on y: {velocity.z}");
    }
    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(groundSpherePosition.position, playerConfig.groundcheckSphereRadius, groundLayer);
        if (isGrounded && velocity.y < 0) {
            velocity.y = playerConfig.constGravity; 
        }
    }

    private void CeilingCheck() {
        isCeilinged = Physics.CheckSphere(ceilingSpherePosition.position, playerConfig.ceilingcheckSphereRadius, ceilingLayer);
        if (isCeilinged && velocity.y > 0) {
            velocity.y = 0;
        }
    }
    public void HandleJump() {
        if (playerConfig.jumpBufferTimer > 0)
        {
            playerConfig.jumpBufferTimer -= Time.deltaTime;
            if (isGrounded) 
            {
                velocity.y = Mathf.Sqrt(playerConfig.jumpHeight * -2f * playerConfig.gravity);
                playerConfig.jumpBufferTimer = 0f;

                if (!isGrounded) {
                    isSprinting = false;
                }
            }
        }
    }
    public void ApplyGravity() {
        if (!isGrounded) {
            velocity.y += playerConfig.gravity * Time.deltaTime;
        }
    }
    // For visualization and debugging purposes only.
    private void OnDrawGizmos()
    {
        if (playerConfig != null && groundSpherePosition != null && ceilingSpherePosition != null) {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundSpherePosition.position, playerConfig.groundcheckSphereRadius);
            Gizmos.color = isCeilinged ? Color.green : Color.red;
            Gizmos.DrawWireSphere(ceilingSpherePosition.position, playerConfig.ceilingcheckSphereRadius);
        }
    }
}
