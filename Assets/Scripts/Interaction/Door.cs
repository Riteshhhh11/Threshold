using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float interactionRange = 3f;
    private bool isOpen = false;

    public float InteractionRange => interactionRange;

    public string GetPrompt() => isOpen ? "Close Door" : "Open Door";
    public bool canInteract() => !isOpen; // Only allow interaction if the door is closed

    public void Interact() {
        isOpen = true;
        transform.parent.Rotate(0, 90f, 0);
        Debug.Log("Door opened!");
    }
   
}
