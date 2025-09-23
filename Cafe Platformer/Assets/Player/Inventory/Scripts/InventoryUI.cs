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
            GameObject slot = Instantiate(inventorySlotGameObject, Vector3.zero, Quaternion.identity, i >= totalSize ? hotbarUI : inventoryUI);

            // Get child image component
            Image image = slot.GetComponent<Image>();

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
            int index = i < Player.Instance.handleInventory.inventory.Length ? i : i - Player.Instance.handleInventory.inventory.Length;
            inventorySlots[i].materialForRendering.SetTexture("_MainTex", Player.Instance.handleInventory.inventory[index].image);
            inventorySlots[i].SetMaterialDirty();
        }

        Debug.Log("Updated inventory UI");
    }

    public void Open()
    {
        hotbarUI.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(true);
    }

    public void Close()
    {
        hotbarUI.gameObject.SetActive(true);
        inventoryUI.gameObject.SetActive(false);
    }
}