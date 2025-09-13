using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int startAmount = 50;
    public Dictionary<string, int> storage = new();

    [HideInInspector] public DrinkData storageSelectedDrink;
    [HideInInspector] public int storageSelectedDrinkIndex = 0;

    MeshRenderer storageVisualMeshRenderer;

    private void Start()
    {
        // Get storage visual mesh renderer
        storageVisualMeshRenderer = transform.Find("StorageVisual").GetComponent<MeshRenderer>();

        // Fill storage
        Dictionary<string, int> fill = GameData.Instance.business.drinks.Select(x => x.name).Zip(Enumerable.Range(0, GameData.Instance.business.drinks.Count).Select(x => startAmount),
        (key, value) => new { Key = key, Value = value }).ToDictionary(item => item.Key, item => item.Value);

        AddStorage(fill);

        // Set initial drink
        ChangeSelectedDrink(0);
    }

    public void AddStorage(Dictionary<string, int> add)
    {
        foreach (KeyValuePair<string, int> pair in add)
        {
            if (storage.ContainsKey(pair.Key))
            {
                storage[pair.Key] += pair.Value;
            }
            else
            {
                storage.Add(pair.Key, pair.Value);
            }
        }
    }

    public void RemoveStorage(Dictionary<string, int> remove)
    {
        foreach (KeyValuePair<string, int> pair in remove)
        {
            if (storage.ContainsKey(pair.Key))
            {
                storage[pair.Key] -= pair.Value;
                Debug.Log("Took " + pair.Value + " " + pair.Key + " from storage. Left: " + storage[pair.Key]);
            }
        }
    }

    public void ChangeSelectedDrink(int change)
    {
        storageSelectedDrinkIndex = Mathf.Clamp(storageSelectedDrinkIndex + change, 0, storage.Keys.Count - 1);
        storageSelectedDrink = GameData.Instance.business.DrinkNameToData(IntToName(storageSelectedDrinkIndex));

        // Change material
        storageVisualMeshRenderer.material.SetFloat("_Index", storageSelectedDrinkIndex);

        Debug.Log($"Storage changed drink from {GameData.Instance.business.DrinkNameToData(IntToName(storageSelectedDrinkIndex - change)).name} to {storageSelectedDrink.name}");
    }

    private string IntToName(int i)
    {
        if (i < 0 || i > storage.Count - 1)
            return "";

        return storage.Keys.ToList()[i];
    }

    public bool HasStorage(string name = "")
    {
        if (name == "")
            name = storageSelectedDrink.name;
        return storage[name] > 0;
    }
}