using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float interactionRange = 3f;
    private bool isOpen = false;
    public float doorOpenAngle = 90f; // Angle to open the door
    public float doorCloseAngle = 0f; // Angle to close the door

    public float InteractionRange => interactionRange;

    public string GetPrompt() => isOpen ? "Close Door" : "Open Door";
    
    public bool canInteract() => true; // Only allow interaction if the door is closed
    //public void Interact() {
    //    isOpen = true;
    //    transform.Rotate(0, 90, 0);
    //    Debug.Log("Door opened!");
    //    //Debug.Log($"{isOpen}");
    //}   

    public void Interact()
    {
        if (isOpen)
        {
            //isOpen = false;
            //transform.Rotate(0, -90, 0);
            //Debug.Log($"is door open? {isOpen}");
            StartCoroutine(RotateDoor(doorCloseAngle));

        }
        else
        {
            //isOpen = true;
            //transform.Rotate(0, 90, 0);
            //Debug.Log($"is door open? {isOpen}");
            StartCoroutine(RotateDoor(doorOpenAngle));
        }
        isOpen = !isOpen; // Toggle the door state after starting the rotation
    }

    private System.Collections.IEnumerator RotateDoor(float angle) {
        Debug.Log($"Rotating door by {angle} degrees");
        Quaternion StartingDoorRotation = transform.rotation;
        Quaternion TargetDoorRotation = StartingDoorRotation * quaternion.Euler(0, angle, 0);
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
