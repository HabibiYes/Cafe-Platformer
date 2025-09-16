using UnityEngine;

public static class MouseLock
{
    // Mouse locked delegate
    public delegate void MouseLocked();
    public static MouseLocked mouseLocked;

    // Mouse unlocked delegate
    public delegate void MouseUnlocked();
    public static MouseUnlocked mouseUnlocked;

    public static void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mouseLocked != null) mouseLocked();
    }

    public static void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (mouseUnlocked != null) mouseUnlocked();
    }
}