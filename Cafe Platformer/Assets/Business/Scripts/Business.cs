using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BusinessSpots))]
public class Business : MonoBehaviour
{
    [HideInInspector] public BusinessSpots businessSpots;
    [HideInInspector] public Supplies supplies;

    public List<Customer> customers { get; private set; } = new();
    public List<Dispenser> dispensers { get; private set; } = new();
    public List<TrashCan> trashCans { get; private set; } = new();
    [field: SerializeField] public List<DrinkData> drinks { get; private set; } = new();

    private void Awake()
    {
        // Get spots
        businessSpots = GetComponent<BusinessSpots>();
        businessSpots.CreateSpots();

        // Get supplies
        supplies = GetComponent<Supplies>();

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
