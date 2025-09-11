using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int startAmount = 50;
    public Dictionary<string, int> storage = new();

    private void Start()
    {
        Dictionary<string, int> fill = GameData.Instance.business.drinks.Select(x => x.name).Zip(Enumerable.Range(0, GameData.Instance.business.drinks.Count).Select(x => startAmount),
        (key, value) => new { Key = key, Value = value }).ToDictionary(item => item.Key, item => item.Value);

        AddStorage(fill);
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
}