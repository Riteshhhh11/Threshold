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
    [SerializeField] private Transform SpherePosition;
    [SerializeField] private LayerMask groundLayer;

    [Header("Player Inputs")]
    [SerializeField] public Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] public bool isGrounded;
    [SerializeField] public bool isSprinting;

    private void Awake()
    {
        PlayerController = GetComponent<CharacterController>();
        playerControls = new PlayerControls(); // Initialize the PlayerControls 
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
        HandleJump();
        if(isSprinting) Debug.Log("Sprinting: " + isSprinting);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        PlayerController.Move(velocity * Time.deltaTime);
    }
    public void HandleMovement(float desiredSpeed) {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y); 
        move = transform.TransformDirection(move); 
        move *= desiredSpeed;
        velocity.x = move.x;
        velocity.z = move.z;
    }
    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(SpherePosition.position, playerConfig.checkSphereRadius, groundLayer);
        if (isGrounded && velocity.y < 0) {
            velocity.y = playerConfig.constGravity; 
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

    //public Vector3 GetHorizontalVelocity(float desiredSpeed) {
    //    Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);
    //    if (inputDirection.sqrMagnitude < 0.01f) {
    //        return Vector3.zero;
    //    }
    //    inputDirection = transform.TransformDirection(inputDirection.normalized);
    //    return inputDirection * desiredSpeed;
    //}
    // For visualization and debugging purposes only.
    private void OnDrawGizmos()
    {
        if (playerConfig != null && SpherePosition != null) {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(SpherePosition.position, playerConfig.checkSphereRadius);
        }
    }
}
