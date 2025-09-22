using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform hotbarUI;
    public Transform inventoryUI;
    [HideInInspector] public Image[] inventorySlots;

    public void Create(int slotCount, int hotbarSize, GameObject inventorySlotGameObject)
    {
        inventorySlots = new Image[slotCount];

        // Create and get each slot image component
        for (int i = 0; i < slotCount + hotbarSize; i++)
        {
            GameObject slot = Instantiate(inventorySlotGameObject, Vector3.zero, Quaternion.identity, i < hotbarSize ? hotbarUI : inventoryUI);

            // Get child image component
            Image image = slot.GetComponent<Image>();
            image.material = new(image.material);
            inventorySlots[i < slotCount ? i : i - slotCount] = image;
        }
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