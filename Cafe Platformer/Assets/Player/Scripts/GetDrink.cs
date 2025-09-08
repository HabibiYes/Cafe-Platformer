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
            dispenser = FindClosestDispenser();

            // Get drink data from dispenser
            if (dispenser != null && player.controls.Player.GetDrink.WasPressedThisFrame())
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

    private Dispenser FindClosestDispenser()
    {
        float closest = Mathf.Infinity;
        Dispenser closestDispenser = null;

        foreach (Dispenser dispenser in dispensers)
        {
            float dist = Vector3.Distance(transform.position, dispenser.transform.position);
            if (dist < closest && dist < range)
            {
                closest = dist;
                closestDispenser = dispenser;
            }
        }

        return closestDispenser;
    }
}
