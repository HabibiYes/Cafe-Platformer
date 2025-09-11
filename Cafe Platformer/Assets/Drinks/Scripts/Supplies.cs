using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Supplies : MonoBehaviour
{
    Dispenser dispenser;

    public Dictionary<string, int> drinkSupplies = new();
    public int maxSupplies = 20;

    private void Start()
    {
        dispenser = GetComponent<Dispenser>();

        // Create fill dict
        Dictionary<string, int> fill = GameData.Instance.business.drinks.Select(x => x.name).Zip(Enumerable.Range(0, GameData.Instance.business.drinks.Count).Select(x => maxSupplies),
        (key, value) => new { Key = key, Value = value }).ToDictionary(item => item.Key, item => item.Value);

        // Fill
        FillSupplies(fill);
    }

    public void FillSupplies(Dictionary<string, int> supplies)
    {
        foreach (KeyValuePair<string, int> supply in supplies)
        {
            if (drinkSupplies.Keys.Contains(supply.Key))
            {
                drinkSupplies[supply.Key] = Mathf.Clamp(drinkSupplies[supply.Key] + supply.Value, 0, maxSupplies);
                Debug.Log("Filled " + supply.Key);
            }
            else
            {
                drinkSupplies.Add(supply.Key, supply.Value);
                Debug.Log("Added " + supply.Key);
            }
        }

        dispenser.UpdateSupplyBar();
    }

    public void RemoveSupply(string name)
    {
        drinkSupplies[name] -= 1;
        dispenser.UpdateSupplyBar();
    }
}