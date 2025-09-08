using UnityEngine;

public class GiveDrink : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;

    private void Awake()
    {
        // Get base player
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.handleDrink.holdingDrink)
        {
            // Find closest customer
            Customer customer = FindClosestValidCustomer();
            
            // Give drink on key press
            if (customer != null && player.controls.Player.GiveDrink.WasPressedThisFrame())
            {
                customer.Remove();

                // Reset holding drink
                player.handleDrink.ResetDrink();

                Debug.Log("Received drink");
            }
        }
    }

    private Customer FindClosestValidCustomer()
    {
        float closest = Mathf.Infinity;
        Customer closestCustomer = null;

        foreach (Customer customer in GameData.Instance.customers)
        {
            float dist = Vector3.Distance(transform.position, customer.transform.position);
            if (dist < closest && dist < range && customer.orderDrink.orderedDrink == player.handleDrink.currentDrink.data)
            {
                closest = dist;
                closestCustomer = customer;
            }
        }

        return closestCustomer;
    }
}