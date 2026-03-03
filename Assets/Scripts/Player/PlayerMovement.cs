using Unity.VisualScripting;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Assets")]
    [SerializeField] private CharacterController PlayerController;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private MovementConfig playerConfig;

    [Header("Ground Check")]
    [SerializeField] private Transform SpherePosition;
    [SerializeField] private LayerMask groundLayer;

    [Header("Player Inputs")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool jumpInput;
    [SerializeField] private bool isGrounded;


    private void Awake()
    {
        PlayerController = GetComponent<CharacterController>();
        playerControls = new PlayerControls(); // Initialize the PlayerControls 
        
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        playerControls.Player.Jump.performed += OnJump; //

    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Player.Jump.performed -= OnJump;
        playerControls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = true; //intent only
    }
    public void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJump();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        PlayerController.Move(velocity * Time.deltaTime);
    }
    public void HandleMovement() {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        move = transform.TransformDirection(move);
        move *= playerConfig.moveSpeed;
        velocity.x = move.x;
        velocity.z = move.z;
    }
    public void GroundCheck() {
        isGrounded = Physics.CheckSphere(SpherePosition.position, playerConfig.checkSphereRadius, groundLayer);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f; 
        }
    }
    public void HandleJump() {
        if (jumpInput && isGrounded) {
            velocity.y = Mathf.Sqrt(playerConfig.jumpHeight * -2f * playerConfig.gravity);
            jumpInput = false; // Reset jump input after processing
        }
    }

    public void ApplyGravity() {
        if (!isGrounded) {
            velocity.y += playerConfig.gravity * Time.deltaTime;
        }
    }
  
    private void OnDrawGizmos()
    {
        if (playerConfig != null && SpherePosition != null) {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(SpherePosition.position, playerConfig.checkSphereRadius);
        }
    }
}
