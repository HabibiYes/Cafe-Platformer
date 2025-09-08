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
            Customer customer = GetObjectFromDistance.FindClosestObject(GameData.Instance.customers, range, transform.position, (customer) => customer.orderDrink.orderedDrink == player.handleDrink.currentDrink.data);
            
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
}