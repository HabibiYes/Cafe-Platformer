using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform hotbarUI;
    [SerializeField] private Transform inventoryUI;
    [HideInInspector] public Image[] inventorySlots;

    private void Awake()
    {
        // Get total number of inventory slots, which is hotbar + inventory
        int slotCount = hotbarUI.childCount + inventoryUI.childCount;

        inventorySlots = new Image[slotCount];

        // Get each slot image component
        for (int i = 0; i < slotCount; i++)
        {
            // Get current transform (hotbar or inventory) based on current index
            Transform currentTransform = i < hotbarUI.childCount ? hotbarUI : inventoryUI;
            int currentIndex = currentTransform == hotbarUI ? i : i - hotbarUI.childCount;

            // Get child image component
            Image image = currentTransform.GetChild(currentIndex).GetComponent<Image>();
            image.material = new(image.material);
            inventorySlots[i] = image;
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