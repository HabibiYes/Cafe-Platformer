using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform hotbarUI;
    public Transform inventoryUI;
    [HideInInspector] public Image[] inventorySlots; // End of slots is hotbar repeat (shows in hotbar object)

    public void Create(int inventorySize, int hotbarSize, GameObject inventorySlotGameObject)
    {
        int totalSize = inventorySize + hotbarSize;
        inventorySlots = new Image[totalSize + hotbarSize];

        // Create and get each slot image component
        for (int i = 0; i < totalSize + hotbarSize; i++)
        {
            GameObject slotGo = Instantiate(inventorySlotGameObject, Vector3.zero, Quaternion.identity, i < totalSize ? inventoryUI : hotbarUI);

            // Get slot component
            InventorySlot slotComponent = slotGo.GetComponent<InventorySlot>();

            // Set slot index and starting data
            slotComponent.index = i < totalSize ? i : i - totalSize;
            slotComponent.data = new HandleInventory.InventoryItem() { name = "None" };

            // Get child image component
            Image image = slotGo.GetComponent<Image>();

            // If index is on repeat of hotbar, set material to reflect hotbar, else, make a new material
            image.material = new(image.material);

            inventorySlots[i] = image;
        }

        UpdateUI();
    }

    public void Destroy()
    {
        // Destroy all slot objects
        foreach (GameObject go in inventorySlots.Select(x => x.gameObject))
        {
            Destroy(go);
        }

        // Clear inventory slots array
        Array.Clear(inventorySlots, 0, inventorySlots.Length);
    }

    public void UpdateUI()
    {
        List<HandleInventory.InventoryItem> inventory = Player.Instance.handleInventory.inventory;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int index = i < inventory.Count ? i : i - inventory.Count;

            InventorySlot slotComponent = inventorySlots[i].GetComponent<InventorySlot>();
            Image slot = inventorySlots[i];

            // Set data
            slotComponent.data = inventory[index];

            // Set material
            slot.materialForRendering.SetTexture("_MainTex", inventory[index].image);
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