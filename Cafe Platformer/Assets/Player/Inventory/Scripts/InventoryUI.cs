using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform hotbarUI;
    public Transform inventoryUI;
    public Image clothingSlot;
    [HideInInspector] public List<Image> inventorySlots; // End of slots is hotbar repeat (shows in hotbar object)

    public void Create(int inventorySize, int hotbarSize, int maxStackSize, GameObject inventorySlotGameObject)
    {
        int totalSize = inventorySize + hotbarSize;
        inventorySlots = new List<Image>();

        // Create and get each slot image component
        for (int i = 0; i < totalSize + hotbarSize; i++)
        {
            // Get parent for slot. If i is less than totalSize set to inventory UI, else is hotbar UI.
            Transform parent = i < totalSize ? inventoryUI : hotbarUI;
            GameObject slotGo = Instantiate(inventorySlotGameObject, Vector3.zero, Quaternion.identity, parent);

            // Get slot component
            InventorySlot slotComponent = slotGo.GetComponent<InventorySlot>();

            // Set slot index
            slotComponent.index = i < totalSize ? i : i - totalSize;

            // Set inventory slots to allow any type, and set max stack size
            slotComponent.allowedType = InventorySlot.AllowedType.Any;
            slotComponent.maxStackSize = maxStackSize;

            // Get child image component
            Image image = slotGo.GetComponent<Image>();

            // If index is on repeat of hotbar, set material to reflect hotbar, else, make a new material
            image.material = new(image.material);

            inventorySlots.Add(image);
        }

        // Set clothing slot
        clothingSlot.material = new(clothingSlot.material);
        InventorySlot clothingSlotComponent = clothingSlot.GetComponent<InventorySlot>();
        clothingSlotComponent.index = Player.Instance.handleInventory.inventory.Count - 1;
        clothingSlotComponent.allowedType = InventorySlot.AllowedType.Wearable;
        clothingSlotComponent.maxStackSize = 1;
        inventorySlots.Add(clothingSlot);

        UpdateUI();
    }

    public void Destroy()
    {
        if (inventorySlots.Count == 0)
            return;

        // Destroy all slot objects, excluding clothing slot
        foreach (GameObject go in inventorySlots.GetRange(0, inventorySlots.Count - 1).Select(x => x.gameObject))
        {
            Destroy(go);
        }

        // Clear inventory slots array
        inventorySlots.Clear();
    }

    public void UpdateUI()
    {
        List<HandleInventory.InventoryItem> inventory = Player.Instance.handleInventory.inventory;

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slotComponent = inventorySlots[i].GetComponent<InventorySlot>();
            Image slot = inventorySlots[i];

            int index = slotComponent.index;

            // Set data
            slotComponent.data = inventory[index];

            // Set material
            slot.material.SetTexture("_MainTex", inventory[index].image);
            slot.SetMaterialDirty();

            // Set count
            slotComponent.data.count = inventory[index].count;
            slotComponent.SetCountText();
        }

        Debug.Log("Updated inventory UI");
    }

    public void Open()
    {
        hotbarUI.gameObject.SetActive(false);
        inventoryUI.parent.gameObject.SetActive(true);
    }

    public void Close()
    {
        hotbarUI.gameObject.SetActive(true);
        inventoryUI.parent.gameObject.SetActive(false);
    }
}