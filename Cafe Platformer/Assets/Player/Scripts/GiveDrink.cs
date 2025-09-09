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
}