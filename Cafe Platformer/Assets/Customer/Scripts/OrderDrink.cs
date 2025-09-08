using System.Collections.Generic;
using UnityEngine;

public class OrderDrink : MonoBehaviour
{
    Customer customer;

    [SerializeField] private List<DrinkData> drinks = new();
    [HideInInspector] public DrinkData orderedDrink;

    private void Awake()
    {
        customer = GetComponent<Customer>();
    }

    public void Order()
    {
        orderedDrink = drinks[Random.Range(0, drinks.Count)];
    }
}