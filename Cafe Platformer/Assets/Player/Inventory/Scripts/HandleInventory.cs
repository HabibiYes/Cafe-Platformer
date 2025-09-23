using System;
using UnityEngine;

public class HandleInventory : MonoBehaviour
{
    Player player;


    // Inventory vars
    public struct InventoryItem
    {
        public string name;
        public Texture2D image;
    }
    public InventoryItem[] inventory = new InventoryItem[10];

    private InventoryItem InventoryDataToStruct(InventoryItemData data)
    {
        // Convert item data to struct
        InventoryItem item = new InventoryItem()
        {
            name = data.name,
            image = data.image
        };

        return item;
    }

    private InventoryUI inventoryUI;
    [HideInInspector] public bool isOpen = false;

    [Header("Settings")]
    [SerializeField] private GameObject inventorySlotGameObject;
    [SerializeField] private int baseInventorySize = 5;
    [SerializeField] private int baseHotbarSize = 5;
    public int maxBagLevel = 3;

    [HideInInspector] public int bagLevel = 1;

    private void Awake()
    {
        player = GetComponent<Player>();

        // Assign open and close input
        player.controls.Player.OpenCloseInventory.performed += (context) => { if (isOpen) Close(); else Open(); };
    }

    private void Start()
    {
        // Get inventory UI
        inventoryUI = GameObject.FindFirstObjectByType<InventoryUI>();
        MakeInventory();

        // Hide inventory at start
        Close();
    }

    // Inventory methods
    public void AddInventory(InventoryItemData data)
    {
        // Check if data is null
        if (data == null)
            return;

        // Get first open index. If out of range, do not add.
        int index = InventoryFirstOpenIndex();
        if (index < 0 || index > inventory.Length)
            return;

        InventoryItem item = InventoryDataToStruct(data);
        inventory[index] = item;

        inventoryUI.UpdateUI();

        Debug.Log("Added " + data.name + " to inventory at index " + index);
    }

    public void EmptyInventory()
    {
        Array.Fill(inventory, new InventoryItem());

        inventoryUI.UpdateUI();

        Debug.Log("Emptied inventory");
    }

    /// <summary>
    /// Finds the first open index in the inventory.
    /// </summary>
    private int InventoryFirstOpenIndex()
    {
        bool found = false;
        int index = -1;
        foreach (InventoryItem item in inventory)
        {
            index++;

            // Check if item is empty
            if (item.name != null)
                continue;

            // If item is empty, choose index
            found = true;
            break;
        }

        if (!found)
            index = -1;
        return index;
    }

    public void MakeInventory()
    {
        // Resize array to match upgrade inventory
        if (inventory.Length != baseInventorySize * bagLevel + baseHotbarSize)
            Array.Resize(ref inventory, baseInventorySize * bagLevel + baseHotbarSize);

        // Remake UI to match upgrade inventory
        inventoryUI.Destroy();
        inventoryUI.Create(baseInventorySize * bagLevel, baseHotbarSize, inventorySlotGameObject);
    }

    private void Open()
    {
        player.DisableMovement();
        inventoryUI.Open();
        isOpen = true;
    }

    private void Close()
    {
        player.EnableMovement();
        inventoryUI.Close();
        isOpen = false;
    }

    // Other methods
    public void UpgradeBag(int change)
    {
        bagLevel = Mathf.Clamp(bagLevel + change, 1, maxBagLevel);
        MakeInventory();
    }
}