using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public List<DrinkData> drinks = new();
    public int selectedDrink = 0;

    public DrinkData GetDrinkData()
    {
        // Return selected drink, or null if out of range
        if (selectedDrink < 0 || selectedDrink > drinks.Count - 1)
            return null;
        return drinks[selectedDrink];
    }
}
