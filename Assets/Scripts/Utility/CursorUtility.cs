using UnityEngine;

public class CursorUtility
{
    public static void LockAndHideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void ConfineAndShowCursor() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static void SetLocked(bool shouldLock) {
        if (shouldLock) {
            LockAndHideCursor();
        }
        else
        {
            UnlockAndShowCursor();
        }
    }
}
