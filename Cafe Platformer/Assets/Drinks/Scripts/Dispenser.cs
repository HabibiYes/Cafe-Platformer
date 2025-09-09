using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public int selectedDrink = 0;

    public DrinkData GetDrinkData()
    {
        // Return selected drink, or null if out of range
        if (selectedDrink < 0 || selectedDrink > GameData.Instance.drinks.Count - 1)
            return null;
        return GameData.Instance.drinks[selectedDrink];
    }

    public void ChangeSelectedDrink(int change)
    {
        selectedDrink = Mathf.Clamp(selectedDrink + change, 0, GameData.Instance.drinks.Count - 1);
        Debug.Log($"Changed drink from {GameData.Instance.drinks[selectedDrink - change].name} to {GameData.Instance.drinks[selectedDrink].name}");
    }
}