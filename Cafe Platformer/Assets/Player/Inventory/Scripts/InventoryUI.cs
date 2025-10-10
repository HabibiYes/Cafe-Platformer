using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform hotbarUI;
    public Transform inventoryUI;
    public Image clothingSlot;
    [HideInInspector] public List<Image> inventorySlots; // End of slots is hotbar repeat (shows in hotbar object)

    [SerializeField] private float selectedScale = 1.25f;
    [SerializeField] private float scaleSpeed = 7f;

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
            InventoryItemUI inventoryBox = slotGo.GetComponent<InventoryItemUI>();

            // Set slot index
            inventoryBox.index = i < totalSize ? i : i - totalSize;

            // Set inventory slots to allow any type, and set max stack size
            inventoryBox.allowedType = InventoryItemUI.AllowedType.Any;
            inventoryBox.maxStackSize = maxStackSize;

            // Get child image component
            Image image = slotGo.GetComponent<Image>();

            // If index is on repeat of hotbar, set material to reflect hotbar, else, make a new material
            image.material = new(image.material);

            inventorySlots.Add(image);
        }

        // Set clothing slot material
        clothingSlot.material = new(clothingSlot.material);

        // Set clothing slot values
        InventoryItemUI clothingSlotComponent = clothingSlot.GetComponent<InventoryItemUI>();
        clothingSlotComponent.index = Player.Instance.handleInventory.inventory.Count - 1;
        clothingSlotComponent.allowedType = InventoryItemUI.AllowedType.Wearable;
        clothingSlotComponent.maxStackSize = 1;

        // Add clothing slot
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
            InventoryItemUI inventoryBox = inventorySlots[i].GetComponent<InventoryItemUI>();
            Image boxUI = inventorySlots[i];

            int index = inventoryBox.index;

            // Set material
            boxUI.material.SetTexture("_MainTex", inventory[index].texture);
            boxUI.SetMaterialDirty();

            // Set count
            inventoryBox.SetCountText(inventory[index].count);
        }

        Debug.Log("Updated inventory UI");
    }

    private void Update()
    {
        // Get pointer position
        PointerEventData data = new(EventSystem.current);
        data.position = Input.mousePosition;

        // Get hits
        List<RaycastResult> hits = new();
        CanvasHandler.Instance.graphicRaycaster.Raycast(data, hits);

        hits = hits.Where(x => x.gameObject.CompareTag("InventoryItemUI")).ToList();

        if (hits.Count > 0)
        {
            // Enlarge selected box
            Transform transform = hits[0].gameObject.transform;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * selectedScale, scaleSpeed * Time.deltaTime);
        }

        // Shrink all non-selected item UIs
        foreach (Transform itemTransform in inventorySlots.Select(x => x.transform))
        {
            if (itemTransform.localScale != Vector3.one && (hits.Count > 0 ? itemTransform != hits[0].gameObject.transform : true))
            {
                itemTransform.localScale = Vector3.Lerp(itemTransform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
            }
        }
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