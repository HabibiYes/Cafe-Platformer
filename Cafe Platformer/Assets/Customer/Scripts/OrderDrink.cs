using System.Collections.Generic;
using UnityEngine;

public class OrderDrink : MonoBehaviour
{
    Customer customer;

    [HideInInspector] public DrinkData orderedDrink;

    private void Awake()
    {
        customer = GetComponent<Customer>();
    }

    public void Order()
    {
        orderedDrink = GameData.Instance.drinks[Random.Range(0, GameData.Instance.drinks.Count)];
    }
}