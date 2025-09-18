using System;
using UnityEngine;

public class HandleInventory : MonoBehaviour
{
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

    private void Start()
    {
        // Empty inventory
        EmptyInventory();
        inventory[0] = InventoryDataToStruct(GameData.Instance.inventoryItems[0]);

        // Get inventory UI
        inventoryUI = GameObject.FindFirstObjectByType<InventoryUI>();

        UpdateInventoryUI();
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
}