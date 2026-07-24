using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float interactionRange = 3f;
    public float doorOpenAngle = 90f; // Angle to open the door
    public float doorCloseAngle = -90f; // Angle to close the door

    [Header("Locked Settings")]
    public bool startLocked = false; // Set to true if the door is locked, false if unlocked
    public string requiredKeyName = "";

    public bool isOpen = false;
    public bool isLocked;
    private Coroutine currentCoroutine;

    public float InteractionRange => interactionRange;

    private void Awake()
    {
        isLocked = startLocked;
    }

    public string GetPrompt() {
        if (isLocked) {
            return $"Locked Door {requiredKeyName}";
        }
        return isOpen ? "Close Door" : "Open Door";

    }
    public bool canInteract()
    {
        if (!isLocked) {
            return true;
        }
        return InventoryManager.Instance.HasKey(requiredKeyName);
    }


    public void TryUnlockDoor() {
        if (InventoryManager.Instance.HasKey(requiredKeyName)) {
            isLocked = false;
            Debug.Log($"Door unlocked with {requiredKeyName}");

            InventoryManager.Instance.RemoveKey(requiredKeyName);
            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(RotateDoor(doorOpenAngle));
            isOpen = true;
        }
        else {
            Debug.Log($"Door is locked. You need {requiredKeyName} to unlock it.");
        }
    }
    public void Interact()
    {
        if (isLocked) {
            TryUnlockDoor();
            return;
        }
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }
        if (isOpen)
        {
            currentCoroutine = StartCoroutine(RotateDoor(doorCloseAngle));
        }
        else
        {
            currentCoroutine = StartCoroutine(RotateDoor(doorOpenAngle));
        }
        isOpen = !isOpen; // Toggle the door state after starting the rotation
    }

    private System.Collections.IEnumerator RotateDoor(float angle) {
        Debug.Log($"Rotating door by {angle} degrees");
        Quaternion StartingDoorRotation = transform.rotation;
        Quaternion TargetDoorRotation = StartingDoorRotation * quaternion.Euler(0, 0, angle);
        float time = 0f;
        float duration = 0.6f;
        while (time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            transform.rotation =  Quaternion.Slerp(StartingDoorRotation, TargetDoorRotation, t);
            yield return null;
        }
        transform.rotation = TargetDoorRotation;
    }
}
