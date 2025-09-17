using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationPriority : MonoBehaviour
{
    Player player;

    [HideInInspector] public string closest = "";

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private (float, float, float, float) GetDistances()
    {
        // Get closest objects
        Customer customer = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.customers, player.interactRange, transform.position);
        Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, player.interactRange, transform.position);
        Storage storage = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.storages, player.interactRange, transform.position);
        TrashCan trashCan = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.trashCans, player.interactRange, transform.position);

        // Get distance to customer
        float distToCustomer = Mathf.Infinity;
        if (customer != null)
            distToCustomer = Vector3.Distance(transform.position, customer.transform.position);

        // Get distance to dispenser
        float distToDispenser = Mathf.Infinity;
        if (dispenser != null)
            distToDispenser = Vector3.Distance(transform.position, dispenser.transform.position);

        // Get distance to storage
        float distToStorage = Mathf.Infinity;
        if (storage != null)
            distToStorage = Vector3.Distance(transform.position, storage.transform.position);

        // Get distance to trash can
        float distToTrashCan = Mathf.Infinity;
        if (trashCan != null)
            distToTrashCan = Vector3.Distance(transform.position, trashCan.transform.position);

        return (distToCustomer, distToDispenser, distToStorage, distToTrashCan);
    }

    private void ChooseClosest(Dictionary<string, float> distances)
    {
        // If all distances are infinity (stations out of range or don't exist),
        // set closest to none and return
        if (distances.All(x => x.Value == Mathf.Infinity))
        {
            closest = "";
            return;
        }

        // Find lowest distance
        float lowestDistance = distances.Values.Min();

        // Get key from index of lowest distance
        int index = Array.IndexOf(distances.Values.ToArray(), lowestDistance);
        string key = distances.Keys.ToList()[index];

        // Set closest to found key
        closest = key;

    }

    private void Update()
    {
        if (player.mode == Player.Mode.Business)
        {
            // Get get distances then find the closest station
            (float distToCustomer, float distToDispenser, float distToStorage, float distToTrashCan) = GetDistances();
            ChooseClosest(new Dictionary<string, float>
            {
                {"Customer", distToCustomer},
                {"Dispenser", distToDispenser},
                {"Storage", distToStorage},
                {"TrashCan", distToTrashCan},
            });

            // Set active scripts according to closest station
            if (closest == "Customer")
                Active(false, true, false, false);
            else if (closest == "Dispenser")
            {
                // If holding box, allow use of storage
                if (player.handleStorage.holdingBox)
                    Active(true, false, true, false);
                else
                    Active(true, true, false, false);
            }
            else if (closest == "Storage")
                Active(false, false, true, false);
            else if (closest == "TrashCan")
                Active(false, false, false, true);
            else
                Active(true, true, true, true);
        }
    }

    private void Active(bool dispenser, bool drink, bool storage, bool trashItem)
    {
        // Change active handling scripts
        player.handleDispenser.enabled = dispenser;
        player.handleDrink.enabled = drink;
        player.handleStorage.enabled = storage;
        player.trashItem.enabled = trashItem;
    }
}