using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [HideInInspector] public Image[] inventorySlots;

    private void Awake()
    {
        inventorySlots = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            image.material = new(image.material);
            inventorySlots[i] = image;
        }
    }
}