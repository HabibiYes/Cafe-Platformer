using System;
using System.Linq;
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
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int index = i < Player.Instance.handleInventory.inventory.Count ? i : i - Player.Instance.handleInventory.inventory.Count;

            // Set data
            inventorySlots[i].GetComponent<InventorySlot>().data = Player.Instance.handleInventory.inventory[index];

            // Set material
            inventorySlots[i].materialForRendering.SetTexture("_MainTex", Player.Instance.handleInventory.inventory[index].image);
            inventorySlots[i].SetMaterialDirty();
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