using System;
using UnityEngine;

public class HandleInventory : MonoBehaviour
{
    public struct InventoryItem
    {
        public string name;
        public Texture2D image;
    }

    public InventoryItem[,] inventory = new InventoryItem[5, 2];

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

    private void Awake()
    {
        EmptyInventory();
        inventory[0, 0] = InventoryDataToStruct(GameData.Instance.inventoryItems[0]);
    }

    private void EmptyInventory()
    {
        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                inventory[y, x] = new InventoryItem();
            }
        }
    }
}