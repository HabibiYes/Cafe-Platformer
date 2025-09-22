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

    private InventoryUI inventoryUI;

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

    [HideInInspector] public bool isOpen = false;

    [SerializeField] private GameObject inventorySlotGameObject;
    [SerializeField] private int slotCount = 10;
    [SerializeField] private int hotbarSize = 5;


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
        inventoryUI.Create(slotCount, hotbarSize, inventorySlotGameObject);

        // Empty inventory
        EmptyInventory();

        // Hide inventory at start
        Close();
    }

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

        UpdateInventoryUI();

        Debug.Log("Added " + data.name + " to inventory at index " + index);
    }

    public void EmptyInventory()
    {
        Array.Fill(inventory, new InventoryItem());

        UpdateInventoryUI();

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

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
        {
            // Get index for inventory to set slot UI
            int index = i < inventoryUI.inventorySlots.Length - inventoryUI.hotbarUI.childCount ? i : i - (inventoryUI.inventorySlots.Length - inventoryUI.hotbarUI.childCount);

            // Set slot UI image
            inventoryUI.inventorySlots[i].material.SetTexture("_MainTex", inventory[index].image);
        }
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
}