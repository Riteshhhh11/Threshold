using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Setting")]
    [SerializeField, Tooltip ("They Key Name must match on the door")]
    private string KeyName = "Trial Key";

    public float InteractionRange => 3f;
    public string GetPrompt() => $"Pick up {KeyName}";
    public bool canInteract() => true;
    public void Interact() {
        Transform playerRoot = GetPlayerRoot();
        if (playerRoot == null)
        {
            Debug.LogWarning("Player root not found. Cannot pick up key.");
            return;
        }
        if (playerRoot.Find(KeyName) == null) {
            GameObject KeyHolder = new GameObject(KeyName);
            KeyHolder.transform.SetParent(playerRoot);
            Debug.Log($"Picked up {KeyName}");
        }
        Destroy(gameObject);
    }
    public Transform GetPlayerRoot() {
        if (PlayerInteraction.Instance != null) {
            return PlayerInteraction.Instance.transform.root;
        }
        GameObject playerObject = GameObject.FindWithTag("Player");
        return playerObject != null ? playerObject.transform : null;
    }

}
