using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<string> collectedKeys = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    public void AddKeys(string keyName) {
        if (!collectedKeys.Contains(keyName)) {
            collectedKeys.Add(keyName);
            foreach (string items in collectedKeys) {
                Debug.Log($"Inventory contains: {items}, ");
            }
            Debug.Log($"Key {keyName} added to inventory.");
        }
    }

    public bool HasKey(string KeyName) {
        //return true if the key is in the inventory, false otherwise
        return collectedKeys.Contains(KeyName); 
    }

    public void RemoveKey(string KeyName) {
        //remove the key from the inventory if it exists
        if (collectedKeys.Contains(KeyName)) {
            collectedKeys.Remove(KeyName);
            Debug.Log($"Key {KeyName} removed from inventory.");
        }
    }
}
