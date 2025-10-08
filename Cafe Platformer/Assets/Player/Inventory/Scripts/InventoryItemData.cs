using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName = "None";

    public HandleInventory.InventoryItem.Type type = HandleInventory.InventoryItem.Type.Valuable;

    public Texture2D texture;

    [Header("If Wearable")]
    public Mesh clothingMesh;
}