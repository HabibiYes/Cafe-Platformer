using System.Collections.Generic;
using System.Linq;
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
        public Texture2D image;
    }
    public List<InventoryItem> inventory = new List<InventoryItem>(10);

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

    private InventoryUI inventoryUI;
    [HideInInspector] public bool isOpen = false;

    [Header("Settings")]
    [SerializeField] private GameObject inventorySlotGameObject;
    [SerializeField] private int baseInventorySize = 5;
    [SerializeField] private int baseHotbarSize = 5;
    public int maxBagLevel = 3;

    [HideInInspector] public int bagLevel = 1;

    [Header("Dragging")]
    [SerializeField] private float maxReleaseDistance = 10f;
    bool isDragging = false;
    InventorySlot draggedItem;

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
        }
    }

    // Inventory methods
    public void AddInventory(InventoryItemData data)
    {
        // Check if data is null
        if (data == null)
            return;

        // Get first open index. If out of range, do not add.
        int index = InventoryFirstOpenIndex();
        if (index < 0 || index > inventory.Count)
            return;

        InventoryItem item = InventoryDataToStruct(data);
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

    public void RemoveInventory(int index)
    {
        if (index < 0 || index >= inventory.Count)
            return;

        // Remove inventory item at index
        inventory[index] = new InventoryItem() { name = "None" };

        inventoryUI.UpdateUI();

        Debug.Log("Removed inventory item at " + index);

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
    private int InventoryFirstOpenIndex()
    {
        bool found = false;
        int index = -1;
        foreach (InventoryItem item in inventory)
        {
            index++;

            // Check if item is empty
            if (item.name != "None")
                continue;

            // If item is empty, choose index
            found = true;
            break;
        }

        if (!found)
            index = -1;
        return index;
    }

    public void MakeInventory()
    {
        // Resize array to match upgrade inventory
        if (inventory.Count != baseInventorySize * bagLevel + baseHotbarSize)
        {
            int count = baseInventorySize * bagLevel + baseHotbarSize - inventory.Count;
            for (int i = 0; i < count; i++)
            {
                inventory.Add(new InventoryItem() { name = "None" });
            }
        }

        // Remake UI to match upgrade inventory
        inventoryUI.Destroy();
        inventoryUI.Create(baseInventorySize * bagLevel, baseHotbarSize, inventorySlotGameObject);
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
        draggedImage.materialForRendering.SetTexture("_MainTex", baseImage.materialForRendering.GetTexture("_MainTex"));

        // Set transform size
        RectTransform draggedTransform = go.GetComponent<RectTransform>();
        RectTransform baseTransform = hits[0].gameObject.GetComponent<RectTransform>();
        draggedTransform.sizeDelta = baseTransform.sizeDelta;

        // Set dragged item and dragged item index
        draggedItem = go.GetComponent<InventorySlot>();
        InventorySlot baseItem = hits[0].gameObject.GetComponent<InventorySlot>();
        draggedItem.index = hits[0].gameObject.GetComponent<InventorySlot>().index;
        draggedItem.data = baseItem.data;

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
            ChangeInventory(closestSlot.index, draggedItem.index, closestSlot.data, draggedItem.data);
        }

        // Destroy dragged item
        Destroy(draggedItem.gameObject);
        draggedItem = null;

        isDragging = false;

        Debug.Log("Stopped dragging item");
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