using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleInventory : MonoBehaviour
{
    Player player;


    // Inventory vars
    [Serializable]
    public struct InventoryItem
    {
        public string name;
        public enum Type
        {
            Valuable = 0,
            Consumable = 1,
            Wearable = 2,
        }
        public Type type;
        public Texture2D image;
        public int count;
    }
    [HideInInspector] public List<InventoryItem> inventory = new List<InventoryItem>(11);

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
    InventorySlot draggedItem;
    InventorySlot baseItem;

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
            draggedItem.data.count = baseItem.data.count;
            draggedItem.SetCountText();
        }
    }

    // Inventory methods
    public void AddInventory(InventoryItemData data, int amount = 1)
    {
        // Check if data is null
        if (data == null)
            return;

        // Get first open index. If out of range, do not add.
        int index = InventoryFirstOpenIndex(data.data);
        if (index < 0 || index > inventory.Count)
            return;

        InventoryItem item = new InventoryItem()
        {
            name = data.data.name,
            image = data.data.image,
            type = data.data.type,
            count = inventory[index].count + amount
        };
        inventory[index] = item;

        inventoryUI.UpdateUI();

        Debug.Log("Added " + data.name + " to inventory at index " + index);
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

        // Remove specified amount of inventory item at index, if none left, remove
        if (inventory[index].count - amount <= 0)
        {
            inventory[index] = new InventoryItem() { name = "None" };

            Debug.Log("Removed inventory item at " + index);
        }
        else
        {
            inventory[index] = new InventoryItem()
            {
                name = inventory[index].name,
                image = inventory[index].image,
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
    private int InventoryFirstOpenIndex(InventoryItem compare)
    {
        // Find all possible indecies
        List<int> foundIndecies = new();
        int i = 0;
        foreach (InventoryItem item in inventory)
        {
            // Check if 'item' and 'compare' are the same item
            if (item.name == compare.name || item.name == "None")
            {
                if (item.count != maxStackSize)
                {
                    foundIndecies.Add(i);
                }
            }

            i++;
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
        if (hits.Count == 0 || !hits.Any(x => x.gameObject.GetComponent<InventorySlot>().data.name != "None"))
            return;

        // Create inventory item to drag
        GameObject go = Instantiate(inventorySlotGameObject, Input.mousePosition, Quaternion.identity, CanvasHandler.Instance.transform);

        // Set image material
        Image draggedImage = go.GetComponent<Image>();
        Image baseImage = hits[0].gameObject.GetComponent<Image>();
        draggedImage.material = new(draggedImage.material);
        draggedImage.material.SetTexture("_MainTex", baseImage.material.GetTexture("_MainTex"));

        // Set transform size
        RectTransform draggedTransform = go.GetComponent<RectTransform>();
        RectTransform baseTransform = hits[0].gameObject.GetComponent<RectTransform>();
        draggedTransform.sizeDelta = baseTransform.sizeDelta;

        // Set dragged item and dragged item index
        draggedItem = go.GetComponent<InventorySlot>();
        baseItem = hits[0].gameObject.GetComponent<InventorySlot>();
        draggedItem.index = hits[0].gameObject.GetComponent<InventorySlot>().index;
        draggedItem.data = baseItem.data;

        // Set dragged item count
        draggedItem.data.count = baseItem.data.count;
        draggedItem.SetCountText();

        isDragging = true;

        Debug.Log("Started dragging item");
    }

    private void StopDragItem()
    {
        // Make sure dragged item exists
        if (!isDragging)
            return;

        // Place dragged item into slot
        List<InventorySlot> slots = GameObject.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None).ToList();
        slots.Remove(draggedItem);
        InventorySlot closestSlot = GetObjectFromDistance.FindClosestObject(slots, maxReleaseDistance, Input.mousePosition);
        if (closestSlot != null)
        {
            if (AllowedInSlot(draggedItem.data, closestSlot))
                ChangeInventory(draggedItem.index, closestSlot.index, draggedItem.data, closestSlot.data);
        }

        // Destroy dragged item
        Destroy(draggedItem.gameObject);
        draggedItem = null;

        // Deselect base item
        baseItem = null;

        isDragging = false;

        Debug.Log("Stopped dragging item");
    }

    private bool AllowedInSlot(InventoryItem item, InventorySlot slot)
    {
        return slot.allowedType == InventorySlot.AllowedType.Any || (InventoryItem.Type)(int)slot.allowedType == item.type;
    }

    private void Open()
    {
        player.DisableMovement();
        inventoryUI.Open();
        isOpen = true;

        player.controls.Player.InventoryDrag.started += (context) => DragItem();
        player.controls.Player.InventoryDrag.canceled += (context) => StopDragItem();

        MouseLock.Unlock();
    }

    private void Close()
    {
        player.EnableMovement();
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