using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer = 1 << 6;
    [SerializeField] private TextMeshProUGUI PromptUI;

    private PlayerControls playerControls;
    private IInteractable currentTarget;
    private Camera playerCamera;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerCamera = Camera.main;
        PromptUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Interact.performed += OnInteract;
    }
    private void OnDisable()
    {
        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Disable();
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact button pressed");
        if (currentTarget != null) {
            Debug.Log("Calling Interact()");
            currentTarget.Interact();
            Debug.Log($"Interacted with {currentTarget}");
            if (PromptUI != null) {
                PromptUI.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("E not pressed");
        }
    }
    private void Update()
    {
        currentTarget = null; //reset target each frame
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //center of screen

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            currentTarget = hit.collider.GetComponent<IInteractable>();
            //Debug.Log("IInteractable found " + (currentTarget != null ? "Yes" : "No"));
            if (currentTarget != null &&
                Vector3.Distance(transform.position, hit.point) <= currentTarget.InteractionRange &&
                currentTarget.canInteract())
            {
                if (PromptUI != null) {
                    PromptUI.text = currentTarget.GetPrompt();
                    PromptUI.gameObject.SetActive(true);
                }
                return;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
        if (PromptUI != null) {
            PromptUI.gameObject.SetActive(false);
        }
    }
}
    


