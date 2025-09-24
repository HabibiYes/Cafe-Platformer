using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] public int index = -1;
    [HideInInspector] public HandleInventory.InventoryItem data;
}