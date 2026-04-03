using UnityEngine;

public class Drawer:MonoBehaviour, IInteractable
{
    [Header("Drawer Settings")]
    [SerializeField] private float openDistance = 1.6f; // Distance the drawer moves when opened    
    [SerializeField] private Vector3 slideDirection = Vector3.back; // Direction the drawer slides (default is backward)
    [SerializeField] private float moveDuration = 0.7f; // Duration of the opening/closing animation
    
    private Vector3 closedPosition;
    private Vector3 openPosition;
    public bool isOpen = false;

    public string GetPrompt() {
       return isOpen ? "Close Drawer" : "Open Drawer";
    }    
    public float InteractionRange => 1f;

    public bool canInteract() => true;
    private void Awake()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + slideDirection.normalized * openDistance; //normalize the direction to ensure consistent movement regardless of the original vector's magnitude
    }
    public void Interact() {
       Vector3 targetPosition = isOpen ? closedPosition : openPosition;
       StartCoroutine(OpenDrawer(targetPosition));
       isOpen = !isOpen;
    }

    private System.Collections.IEnumerator OpenDrawer(Vector3 targetPosition) {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        transform.localPosition = targetPosition; // Ensure it ends at the exact target position
    }

}
