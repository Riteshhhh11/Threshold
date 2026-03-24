using UnityEngine;

public interface IInteractable
{
    float InteractionRange { get; } //ray cast range for interaction
    string GetPrompt(); //display a prompt when in interaction range
    bool canInteract(); //Check if the player can interact with the object
    void Interact(); //perform interaction

    //void OpenDoor();

    //void CloseDoor();
}
