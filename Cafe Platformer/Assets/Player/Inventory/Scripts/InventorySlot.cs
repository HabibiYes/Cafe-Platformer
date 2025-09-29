using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] public int index = -1;
    [HideInInspector] public HandleInventory.InventoryItem data;
    public enum AllowedType
    {
        Valuable = 0,
        Consumable = 1,
        Wearable = 2,
        Any = 3,
    }
    [HideInInspector] public AllowedType allowedType = AllowedType.Any;
    [HideInInspector] public int maxStackSize = 20;

    [HideInInspector] public TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void SetCountText()
    {
        tmpText.text = data.count > 0 ? data.count.ToString() : "";
    }
}