using UnityEngine;
using UnityEngine.InputSystem;

// DONT COMMIT

public class DebugMode : MonoBehaviour
{
    [HideInInspector] public bool isEnabled = false;

    [Header("Inventory")]
    [SerializeField] private InventoryItemData testData;
    [SerializeField] private InventoryItemData testData2;

    private void Update()
    {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            isEnabled = !isEnabled;
            if (isEnabled) Enable(); else Disable();
        }
    }

    private void Enable()
    {
        Player.Instance.controls.Player.Interact.performed += (context) => Player.Instance.handleInventory.AddInventory(testData);
        Player.Instance.controls.Player.AltInteract.performed += (context) => Player.Instance.handleInventory.AddInventory(testData2);

        Debug.Log("Enabled debug mode");
    }

    private void Disable()
    {
        Player.Instance.controls.Player.Interact.performed -= (context) => Player.Instance.handleInventory.AddInventory(testData);
        Player.Instance.controls.Player.AltInteract.performed -= (context) => Player.Instance.handleInventory.AddInventory(testData2);

        Debug.Log("Disabled debug mode");
    }
}