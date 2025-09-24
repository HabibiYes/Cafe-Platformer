using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] public int index = -1;
    [HideInInspector] public HandleInventory.InventoryItem data;

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