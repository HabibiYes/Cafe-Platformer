using System;
using UnityEngine;

public class HandleDrink : MonoBehaviour
{
    Player player;

    [Header("Get Drink")]
    [SerializeField] private GameObject cup;

    [HideInInspector] public bool holdingDrink = false;
    [HideInInspector] public Drink currentDrink;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void SetDrink(Drink drink)
    {
        holdingDrink = true;
        currentDrink = drink;

        Debug.Log("Got drink");
    }

    public void ResetDrink()
    {
        holdingDrink = false;

        Destroy(currentDrink.gameObject);
        currentDrink = null;
    }

    private void Update()
    {
        if (!holdingDrink && !player.handleStorage.holdingBox)
        {
            GetDrink();
        }
        else if (holdingDrink)
        {
            GiveDrink();
        }
    }

    private void GetDrink()
    {
        // Find closest dispenser
        Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, player.interactRange, transform.position);

        if (dispenser != null && dispenser.supplies.drinkSupplies[dispenser.GetDrinkData().name] > 0 && player.controls.Player.Interact.WasPressedThisFrame())
        {
            // Create cup instance
            GameObject go = Instantiate(cup, transform.position + player.playerModel.forward, Quaternion.identity, player.playerModel);

            // Set drink data to dispenser data
            Drink drink = go.GetComponent<Drink>();
            drink.data = dispenser.GetDrinkData();

            // Set holding drink
            player.handleDrink.SetDrink(drink);

            // Remove one supplies
            dispenser.supplies.RemoveSupply(drink.data.name);
        }
    }

    private void GiveDrink()
    {
        // Find closest customer
        Customer customer = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.customers, player.interactRange, transform.position, (customer) => customer.orderDrink.orderedDrink == player.handleDrink.currentDrink.data);

        // Give drink on key press
        if (customer != null && player.controls.Player.Interact.WasPressedThisFrame())
        {
            // Receive money
            player.AddMoney(player.handleDrink.currentDrink.data.price);

            // Reset holding drink
            player.handleDrink.ResetDrink();

            // Remove customer
            customer.Remove();

            Debug.Log("Received drink");
        }
    }
}