using System;
using UnityEngine;

public class HandleDrink : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;

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
        if (!holdingDrink)
        {
            GetDrink();
        }
        else
        {
            GiveDrink();
        }
    }

    private void GetDrink()
    {
        // Find closest dispenser
        Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, range, transform.position);

        if (dispenser != null && player.controls.Player.Interact.WasPressedThisFrame())
        {
            // Create cup instance
            GameObject go = Instantiate(cup, transform.position + player.playerModel.forward, Quaternion.identity, player.playerModel);

            // Set drink data to dispenser data
            Drink drink = go.GetComponent<Drink>();
            drink.data = dispenser.GetDrinkData();

            // Set holding drink
            player.handleDrink.SetDrink(drink);
        }
    }

    private void GiveDrink()
    {
        // Find closest customer
        Customer customer = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.customers, range, transform.position, (customer) => customer.orderDrink.orderedDrink == player.handleDrink.currentDrink.data);

        // Give drink on key press
        if (customer != null && player.controls.Player.Interact.WasPressedThisFrame())
        {
            // Reset holding drink
            player.handleDrink.ResetDrink();

            // Remove customer
            customer.Remove();

            Debug.Log("Received drink");
        }
    }
}