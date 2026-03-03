using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera:MonoBehaviour
{
    [SerializeField] CameraConfig cameraConfig;
    [SerializeField] PlayerControls playerControls;
    [SerializeField] private Vector2 mouseDelta;
    [SerializeField] public float currentPitch;


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Look.performed += lookInput;
        playerControls.Player.Look.canceled += lookInput;
    }

    private void OnDisable()
    {
        playerControls.Player.Look.performed -= lookInput;
        playerControls.Player.Look.canceled -= lookInput;
        playerControls.Disable();
    }

    private void lookInput(InputAction.CallbackContext context) {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector2 delta = mouseDelta; //Getting the mouse input this frame
        float horizontalRotation = delta.x * cameraConfig.mouseSensitivity * Time.deltaTime; //Calculating the horizontal rotation based on mouse input and multiplying by sensitivity to scale it and deltaTime for frame rate independence
        float verticalRotation = delta.y * cameraConfig.mouseSensitivity * Time.deltaTime; //Calculating the vertical rotation based on mouse input and multiplying by sensitivity to scale it and deltaTime for frame rate independence
        transform.parent.Rotate(0, horizontalRotation, 0); //Rotating the parent object (the player) horizontally based on the calculated horizontal rotation
        currentPitch -= verticalRotation;
        currentPitch = Mathf.Clamp(
            currentPitch,
            cameraConfig.ClampPitchMin,
            cameraConfig.ClampPitchMax
         );
        transform.localRotation = Quaternion.Euler(currentPitch, 0, 0);
    }   
}
