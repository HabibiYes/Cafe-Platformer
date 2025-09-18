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

    public InventoryItem[] inventory = new InventoryItem[5];

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


    private void Awake()
    {
        player = GetComponent<Player>();

        // Assign open and close input
        player.controls.Player.OpenCloseInventory.performed += (context) => { if (isOpen) Close(); else Open(); };
    }

    private void Start()
    {
        // Empty inventory
        EmptyInventory();

        // Get inventory UI
        inventoryUI = GameObject.FindFirstObjectByType<InventoryUI>();

        UpdateInventoryUI();

        // Hide inventory at start
        Close();
    }

    private void EmptyInventory()
    {
        Array.Fill(inventory, new InventoryItem());
    }

    private void UpdateInventoryUI()
    {
        // TODO: Fix inventory slot material changing
        for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
        {
            inventoryUI.inventorySlots[i].material.SetTexture("_MainTex", inventory[i].image);
        }
    }

    private void Open()
    {
        player.DisableMovement();
        inventoryUI.gameObject.SetActive(true);
        isOpen = true;
    }

    private void Close()
    {
        player.EnableMovement();
        inventoryUI.gameObject.SetActive(false);
        isOpen = false;
    }
}