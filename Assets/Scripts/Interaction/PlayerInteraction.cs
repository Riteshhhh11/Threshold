using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer = 1 << 7; 
    [SerializeField] private TextMeshProUGUI PromptUI;

    private PlayerControls playerControls;
    private IInteractable currentTarget; 
    private Camera playerCamera;

    public static PlayerInteraction Instance { get; private set; }

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerCamera = Camera.main;
        PromptUI.gameObject.SetActive(false);
        PlayerInteraction.Instance = this;
        Debug.Log("PlayerInteraction instance created");
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
        if (currentTarget != null) {
            currentTarget.Interact();
        }
       
    }
    private void Update()
    {
        currentTarget = null; //reset target each frame

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //center of screen

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null) { 
                currentTarget = interactable;
                float distanceToHit = Vector3.Distance(transform.position, hit.point);
                if (PromptUI != null)
                {
                    PromptUI.text = currentTarget.GetPrompt();
                    PromptUI.color = currentTarget.canInteract()? Color.red : Color.red; // Gray out prompt if interaction is not possible
                    PromptUI.gameObject.SetActive(true);
                    //Debug.Log($"Prompt displayed: {currentTarget.GetPrompt()}");
                }
                return;
            }
        }
        else{
            Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
            if (PromptUI != null)
            {
                PromptUI.gameObject.SetActive(false);
            }
        }
        
    }
}
    


