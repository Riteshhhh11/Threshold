using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour, IInteractable
{
    private int currentIndex = 0;
    public int[] keyPadInputNumber = new int[6];
    public int[] codeCombination = new int[6]; // Example code combination
    private bool isLocked;
    private bool isOpen = false;
    public float doorOpenAngle = 90f; // Angle to open the safe
    public float doorCloseAngle = -90f; // Angle to close the safe
    public GameObject keyPadPanel;
    public TextMeshProUGUI codeText;
    public float InteractionRange => 3f; // Set the interaction range for the keypad
    private void Awake()
    {
        if (keyPadPanel == null) {
            keyPadPanel = GameObject.Find("KeyPadPanel");
        }
        if (keyPadPanel != null) {
            keyPadPanel.SetActive(false); // Hide the keypad panel at the start
        }
        isLocked = true; // The safe starts locked
    }
    public string GetPrompt() {
        if (isLocked) {
            return "Enter Code";
        }
        return isOpen ? "Close Safe" : "Open Safe";
    }
    public bool canInteract()
    {
        return true;
    }

    public void Interact() {
        if (isLocked) {
            ShowKeypad();
            return;
        }
        if (isOpen) {
            StartCoroutine(OpenSafe(doorCloseAngle));
            isOpen = false;
        }
        else
        {
            StartCoroutine(OpenSafe(doorOpenAngle));
            isOpen = true;
        }
    }

    private void UnlockSafe() {
        isLocked = false;
        if (keyPadPanel != null) {
            keyPadPanel.SetActive(false); // Hide the keypad panel after unlocking
        }
        StartCoroutine(OpenSafe(doorOpenAngle)); // Open the safe immediately after unlocking
        isOpen = true;
    }
    public void OnNumberPressed(int number) { //function to add array on the button pressed
        if (currentIndex < keyPadInputNumber.Length) {
            keyPadInputNumber[currentIndex] = number;
            currentIndex++;
            UpdateDisplay();
        }
    }

    public void OnCodeCheck() {
        if (currentIndex < keyPadInputNumber.Length) {
            Debug.Log("Not enough numbers entered.");
            return;
        }
        bool isCodeCorrect = CheckCombination(keyPadInputNumber);
        if (isCodeCorrect)
        {
            UnlockSafe();
            Debug.Log("Safe unlocked!");
        }
        else {
            ResetInput();
        }
    }

    public bool CheckCombination(int[] enteredCode) {
        if (enteredCode.Length != codeCombination.Length) {
            return false; // Length mismatch, code is incorrect
        }
        for (int i = 0; i < codeCombination.Length; i++) {
            if (enteredCode[i] != codeCombination[i]) { 
                return false; // If any number doesn't match, the code is incorrect
            }
        }
        return true; // All numbers match, the code is correct
    }

    private void ShowKeypad() {
        if (keyPadPanel != null)
        {
            keyPadPanel.SetActive(true); // Show the keypad panel
            CursorUtility.UnlockAndShowCursor();
            ResetInput();
        }
        else {
            CursorUtility.LockAndHideCursor();
        }
    }

    private void UpdateDisplay()
    {
        if (codeText == null) {
            return;
        }
        string display = "";
        for (int i = 0; i < keyPadInputNumber.Length; i++) {
            display += (i < currentIndex) ? keyPadInputNumber[i].ToString() : "_"; // Show entered numbers and underscores for remaining slots
        }
        codeText.SetText(display);
    }

    public void ResetInput() {
        currentIndex = 0;
        for (int i = 0; i < keyPadInputNumber.Length; i++) {
            keyPadInputNumber[i] = 0;
        }
        UpdateDisplay();
    }
    public void onClearPressed() {
        ResetInput();
    }

    private System.Collections.IEnumerator OpenSafe(float angle)
    {
        Quaternion startingRotation = transform.rotation;
        Quaternion targetRotation = startingRotation * Quaternion.Euler(0, -angle, 0);
        float time = 0;
        float duration = 0.6f; // Duration of the opening animation
        while (time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, t);
            yield return null; // Placeholder for the actual opening logic
        }
        transform.rotation = targetRotation; // Ensure it ends at the exact target rotation
    }
}
