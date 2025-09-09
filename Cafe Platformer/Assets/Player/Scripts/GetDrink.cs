using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetDrink : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;
    [SerializeField] private GameObject cup;

    private List<Dispenser> dispensers = new();
    Dispenser dispenser;

    private void Awake()
    {
        // Get base player
        player = GetComponent<Player>();

        // Get all dispensers on scene change
        SceneManager.sceneLoaded += (a, b) => { if (a.name == "Business") { dispensers = GameObject.FindObjectsByType<Dispenser>(FindObjectsSortMode.None).ToList(); } };
    }

    private void Update()
    {
        if (!player.handleDrink.holdingDrink)
        {
            // Get closest dispenser
            dispenser = GetObjectFromDistance.FindClosestObject(dispensers, range, transform.position);

            // Get drink data from dispenser
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
    }
}
