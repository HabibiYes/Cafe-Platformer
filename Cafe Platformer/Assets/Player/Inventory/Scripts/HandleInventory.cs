using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleInventory : MonoBehaviour
{
    Player player;


    // Inventory vars
    public struct InventoryItem
    {
        public string name;

        public enum Type
        {
            None = 0,
            Valuable = 1,
            Consumable = 2,
            Wearable = 3,
        }
        public Type type;

        public Texture2D texture;

        public int count;

        // If wearable
        public Mesh clothingMesh;
    }
    [HideInInspector] public List<InventoryItem> inventory = new(11);

    public InventoryItem InventoryItemDataToStruct(InventoryItemData data, int count = 0)
    {
        InventoryItem item = new()
        {
            name = data.itemName,
            type = data.type,
            texture = data.texture,
            count = count,

            clothingMesh = data.clothingMesh
        };
        return item;
    }

    private InventoryUI inventoryUI;
    [HideInInspector] public bool isOpen = false;

    [Header("Settings")]
    [SerializeField] private GameObject inventorySlotGameObject;
    [SerializeField] private int baseInventorySize = 5;
    [SerializeField] private int baseHotbarSize = 5;
    [SerializeField] private int maxStackSize = 20;
    public int maxBagLevel = 3;

    [HideInInspector] public int bagLevel = 1;

    [Header("Dragging")]
    [SerializeField] private float maxReleaseDistance = 10f;
    bool isDragging = false;
    InventoryItemUI draggedItem;
    InventoryItemUI baseItem;

    // Clothing
    public delegate void ClothingChanged(Mesh mesh);
    public ClothingChanged clothingChanged;

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

    private void Update()
    {
        if (isDragging)
        {
            draggedItem.transform.position = Input.mousePosition;
            draggedItem.SetCountText(inventory[baseItem.index].count);
        }
    }

    // Inventory methods
    public void AddInventory(InventoryItemData itemData, int amount = 1)
    {
        // Check if data is null
        if (itemData == null)
            return;

        // Get first open index. If out of range, do not add.
        int index = InventoryFirstValidIndex(InventoryItemDataToStruct(itemData));
        if (index < 0 || index > inventory.Count)
            return;

        InventoryItem item = InventoryItemDataToStruct(itemData, inventory[index].count + 1);
        inventory[index] = item;

        inventoryUI.UpdateUI();

        Debug.Log("Added " + item.name + " to inventory at index " + index);
    }

    public void ChangeInventory(int index1, int index2, InventoryItem data1, InventoryItem data2)
    {
        // Replace item at index
        inventory[index1] = data2;
        inventory[index2] = data1;

        inventoryUI.UpdateUI();

        Debug.Log("Swapped inventory indecies " + index1 + " and " + index2);
    }

    public void RemoveInventory(int index, int amount)
    {
        if (index < 0 || index >= inventory.Count)
            return;

        // Remove specified amount of inventory item at index, if none left, set slot to empty
        if (inventory[index].count - amount <= 0)
        {
            inventory[index] = new InventoryItem()
            {
                name = "None",
                type = InventoryItem.Type.None,
            };

            Debug.Log("Removed inventory item at " + index);
        }
        else
        {
            inventory[index] = new InventoryItem()
            {
                name = inventory[index].name,
                texture = inventory[index].texture,
                count = inventory[index].count - amount
            };

            Debug.Log("Removed " + amount + " of " + inventory[index].name + " at " + index);
        }

        inventoryUI.UpdateUI();

    }

    public void EmptyInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i] = new InventoryItem() { name = "None" };
        }

        inventoryUI.UpdateUI();

        Debug.Log("Emptied inventory");
    }

    /// <summary>
    /// Finds the first open index in the inventory.
    /// </summary>
    private int InventoryFirstValidIndex(InventoryItem compare)
    {
        // Find all possible indecies, excluding clothing slot
        List<int> foundIndecies = new();
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            InventoryItem item = inventory[i];

            // Check if 'item' and 'compare' are the same item
            if (item.name == compare.name || item.name == "None")
            {
                // Get current count of inventory slot
                int count = inventory[i].count;

                // Check if count is less than max stack size, or is zero is wearable
                if (count != (item.type == InventoryItem.Type.Wearable ? 1 : maxStackSize))
                {
                    foundIndecies.Add(i);
                }
            }
        }

        if (foundIndecies.Count == 0)
            return -1;

        // Try and find first stack to combine with, then return that index
        try
        {
            return foundIndecies.First(x => inventory[x].name != "None");
        }
        // If no stack is found, choose the first empty slot
        catch (InvalidOperationException)
        {
            try
            {
                return foundIndecies.First(x => inventory[x].name == "None");
            }
            // If no empty slot is found, don't add
            catch
            {
                return -1;
            }
        }
    }

    public void MakeInventory()
    {
        // Resize array to match upgrade inventory
        if (inventory.Count != baseInventorySize * bagLevel + baseHotbarSize)
        {
            // Create regular spots
            int count = baseInventorySize * bagLevel + baseHotbarSize - inventory.Count;
            for (int i = 0; i < count; i++)
            {
                inventory.Add(new InventoryItem() { name = "None" });
            }

            // Create clothing spot
            inventory.Add(new InventoryItem() { name = "None", type = InventoryItem.Type.Wearable });
        }

        // Remake UI to match upgrade inventory
        inventoryUI.Destroy();
        inventoryUI.Create(baseInventorySize * bagLevel, baseHotbarSize, maxStackSize, inventorySlotGameObject);
    }

    private void DragItem()
    {
        // Get pointer data
        PointerEventData data = new(EventSystem.current);
        data.position = Input.mousePosition;

        // Raycast onto canvas, looking for inventory items
        List<RaycastResult> hits = new();
        CanvasHandler.Instance.graphicRaycaster.Raycast(data, hits);

        hits = hits.Where(x => x.gameObject.CompareTag("InventoryItemUI")).ToList();

        // Return if nothing was hit or none of the hits were valid
        if (hits.Count == 0 || !hits.Any(x => inventory[x.gameObject.GetComponent<InventoryItemUI>().index].name != "None"))
            return;

        // Create inventory item to drag
        GameObject go = Instantiate(inventorySlotGameObject, Input.mousePosition, Quaternion.identity, CanvasHandler.Instance.transform);

        // Set transform size
        RectTransform draggedTransform = go.GetComponent<RectTransform>();
        RectTransform baseTransform = hits[0].gameObject.GetComponent<RectTransform>();
        draggedTransform.sizeDelta = baseTransform.sizeDelta;

        // Set image material
        Image draggedImage = go.GetComponent<Image>();
        Image baseImage = hits[0].gameObject.GetComponent<Image>();
        draggedImage.material = new(draggedImage.material);
        draggedImage.material.SetTexture("_MainTex", baseImage.material.GetTexture("_MainTex"));

        // Set dragged item and dragged item index
        draggedItem = go.GetComponent<InventoryItemUI>();
        baseItem = hits[0].gameObject.GetComponent<InventoryItemUI>();
        draggedItem.index = hits[0].gameObject.GetComponent<InventoryItemUI>().index;

        isDragging = true;

        Debug.Log("Started dragging item");
    }

    private void StopDragItem()
    {
        // Make sure dragged item exists
        if (!isDragging)
            return;

        // Place dragged item into slot
        List<InventoryItemUI> slots = GameObject.FindObjectsByType<InventoryItemUI>(FindObjectsSortMode.None).ToList();
        slots.Remove(draggedItem);
        InventoryItemUI closestSlot = GetObjectFromDistance.FindClosestObject(slots, maxReleaseDistance, Input.mousePosition);
        if (closestSlot != null)
        {
            Debug.Log(inventory[draggedItem.index].type.HumanName());
            Debug.Log(((InventoryItem.Type)(int)closestSlot.allowedType).HumanName());
            if (SlotTypeMatches(inventory[draggedItem.index], closestSlot))
            {
                Debug.Log("Slot is valid");
                ChangeInventory(draggedItem.index, closestSlot.index, inventory[draggedItem.index], inventory[closestSlot.index]);

                // Call clothing change
                clothingChanged?.Invoke(inventory[inventory.Count - 1].clothingMesh);
            }
        }

        // Destroy dragged item
        Destroy(draggedItem.gameObject);
        draggedItem = null;

        // Deselect base item
        baseItem = null;

        isDragging = false;

        Debug.Log("Stopped dragging item");
    }

    private bool SlotTypeMatches(InventoryItem item, InventoryItemUI slot)
    {
        return slot.allowedType == InventoryItemUI.AllowedType.Any || (InventoryItem.Type)(int)slot.allowedType == item.type;
    }

    private void Open()
    {
        player.playerMovement.DisableAllMovement();
        inventoryUI.Open();
        isOpen = true;

        player.controls.Player.InventoryDrag.started += (context) => DragItem();
        player.controls.Player.InventoryDrag.canceled += (context) => StopDragItem();

        MouseLock.Unlock();
    }

    private void Close()
    {
        player.playerMovement.EnableAllMovement();
        inventoryUI.Close();
        isOpen = false;

        player.controls.Player.InventoryDrag.started -= (context) => DragItem();
        player.controls.Player.InventoryDrag.canceled -= (context) => StopDragItem();

        MouseLock.Lock();
    }

    // Other methods
    public void UpgradeBag(int change)
    {
        bagLevel = Mathf.Clamp(bagLevel + change, 1, maxBagLevel);
        MakeInventory();
    }
}