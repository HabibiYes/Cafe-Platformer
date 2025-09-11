using UnityEngine;

[RequireComponent(typeof(Supplies))]
public class Dispenser : MonoBehaviour
{
    public int selectedDrink = 0;
    [SerializeField] private int containerMaterialIndex = 0;

    [HideInInspector] public Supplies supplies;

    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        supplies = GetComponent<Supplies>();
    }

    public DrinkData GetDrinkData()
    {
        // Return selected drink, or null if out of range
        if (selectedDrink < 0 || selectedDrink > GameData.Instance.business.drinks.Count - 1)
            return null;
        return GameData.Instance.business.drinks[selectedDrink];
    }

    public void ChangeSelectedDrink(int change)
    {
        // Change drink
        selectedDrink = Mathf.Clamp(selectedDrink + change, 0, GameData.Instance.business.drinks.Count - 1);

        // Change container material
        meshRenderer.materials[containerMaterialIndex].color = GameData.Instance.business.drinks[selectedDrink].color;

        Debug.Log($"Changed drink from {GameData.Instance.business.drinks[selectedDrink - change].name} to {GameData.Instance.business.drinks[selectedDrink].name}");
    }
}