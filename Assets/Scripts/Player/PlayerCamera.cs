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

    private void LateUpdate()
    {
        //Raw delta input
        float horizontalRotation = mouseDelta.x * cameraConfig.mouseSensitivity * Time.deltaTime; //Calculating the horizontal rotation based on mouse input and multiplying by sensitivity to scale it and deltaTime for frame rate independence
        float verticalRotation = mouseDelta.y * cameraConfig.mouseSensitivity * Time.deltaTime; //Calculating the vertical rotation based on mouse input and multiplying by sensitivity to scale it and deltaTime for frame rate independence

        //Accumulating the total rotation
        cameraConfig.yaw += horizontalRotation;
        currentPitch -= verticalRotation;
        currentPitch = Mathf.Clamp(
            currentPitch,
            cameraConfig.ClampPitchMin,
            cameraConfig.ClampPitchMax
         );

        //Target rotation from the accumulated rotation
        Quaternion targetBodyRotation = Quaternion.Euler(0f, cameraConfig.yaw, 0);
        Quaternion targetCameraRotaion = Quaternion.Euler(currentPitch, 0, 0);
        Debug.Log("Target Body Rotation: " + targetBodyRotation.eulerAngles);
        Debug.Log("Target Camera Rotation: " + targetCameraRotaion.eulerAngles);

        //Slerp for smooth rotation 
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, targetBodyRotation, cameraConfig.cameraSmoothing * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetCameraRotaion, cameraConfig.cameraSmoothing * Time.deltaTime);
    }
}
