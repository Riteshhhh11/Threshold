using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Setting")]
    [SerializeField, Tooltip ("They Key Name must match on the door")]
    private string KeyName = "Trial Key";

    public float InteractionRange => 3f;
    public string GetPrompt() => $"Pick up {KeyName}";
    public bool canInteract() => true;
    public void Interact(Transform interactor) {
        InventoryManager.Instance.AddKeys(KeyName);
        Destroy(gameObject);
    }
}
