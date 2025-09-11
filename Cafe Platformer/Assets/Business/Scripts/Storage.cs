using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int capacity = 50;
    public int amount = 0;
    public Dictionary<string, int> storage = new();

    private void AddStorage(Dictionary<string, int> add)
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

    private void RemoveStorage(Dictionary<string, int> remove)
    {
        foreach (KeyValuePair<string, int> pair in remove)
        {
            if (storage.ContainsKey(pair.Key))
                storage[pair.Key] -= pair.Value;
        }
    }
}