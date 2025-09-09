using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public List<Customer> customers { get; private set; } = new();
    public List<Dispenser> dispensers { get; private set; } = new();
    public List<TrashCan> trashCans { get; private set; } = new();

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Get objects on scene change
        SceneManager.sceneLoaded += (a, b) => { if (a.name == "Business") { dispensers = GameObject.FindObjectsByType<Dispenser>(FindObjectsSortMode.None).ToList(); } };
        SceneManager.sceneLoaded += (a, b) => { if (a.name == "Business") { trashCans = GameObject.FindObjectsByType<TrashCan>(FindObjectsSortMode.None).ToList(); } };
    }

    public void AddCustomer(Customer customer)
    {
        customers.Add(customer);
    }

    public void RemoveCustomer(Customer customer)
    {
        customers.Remove(customer);
    }
}