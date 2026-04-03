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
        Transform playerRoot = GetPlayerRoot();
        return playerRoot != null && playerRoot.Find(requiredKeyName) != null;
    }

    public Transform GetPlayerRoot() {
        if (PlayerInteraction.Instance != null) {
            return PlayerInteraction.Instance.transform.root;
        }
        GameObject playerObject = GameObject.FindWithTag("Player");
        return playerObject != null ? playerObject.transform : null;
    }

    public void TryUnlockDoor() {
        Transform playerRoot = GetPlayerRoot();
        if(playerRoot == null){
            return;
        }
        if (playerRoot.Find(requiredKeyName) != null)
        {
            isLocked = false;
            Debug.Log($"Door unlocked with {requiredKeyName}");
            Transform KeyHolder = playerRoot.Find(requiredKeyName);
            if (KeyHolder != null)
            {
                Destroy(KeyHolder.gameObject);
            }
            StartCoroutine(RotateDoor(doorOpenAngle));
            isOpen = true;
        }
        else {
            Debug.Log($"Player does not have the required key: {requiredKeyName}");
        }
    }
    public void Interact()
    {
        if (isLocked) {
            TryUnlockDoor();
            return;
        }

        if (isOpen)
        {
            StartCoroutine(RotateDoor(doorCloseAngle));
        }
        else
        {
            StartCoroutine(RotateDoor(doorOpenAngle));
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
