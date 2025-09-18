using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public Business business { get; private set; }

    public List<InventoryItemData> inventoryItems { get; private set; } = new();

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Get business
        SceneManager.sceneLoaded += (a, b) => { if (a.name == "Business") business = GameObject.FindFirstObjectByType<Business>(); };
    }
}