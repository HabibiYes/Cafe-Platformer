using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    [HideInInspector] public int index = -1;

    public enum AllowedType
    {
        Any = 0,
        Valuable = 1,
        Consumable = 2,
        Wearable = 3,
    }
    [HideInInspector] public AllowedType allowedType = AllowedType.Any;

    [HideInInspector] public int maxStackSize = 20;

    [HideInInspector] public TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void SetCountText(int count)
    {
        // Set count text. If zero, no text.
        tmpText.text = count > 0 ? count.ToString() : "";
    }
}