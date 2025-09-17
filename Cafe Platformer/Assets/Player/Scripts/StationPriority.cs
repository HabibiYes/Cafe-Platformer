using System.Collections.Generic;
using UnityEngine;

public class StationPriority : MonoBehaviour
{
    Player player;

    [HideInInspector] public string closest = "";

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private List<Station> StationChecks(List<Station> stations)
    {
        List<Station> approvedStations = new(stations);

        foreach (Station station in stations)
        {
            if ((!station.allowHoldingDrink && player.handleDrink.holdingDrink) || (!station.allowHoldingBox && player.handleStorage.holdingBox))
                approvedStations.Remove(station);
        }

        return approvedStations;
    }

    private void Update()
    {
        if (player.mode == Player.Mode.Business)
        {
            // Get closest station, then set closest to station name, if not null
            Station station = GetObjectFromDistance.FindClosestObject(StationChecks(GameData.Instance.business.stations), player.interactRange, transform.position);
            closest = station != null ? station.name : "";
            Debug.Log(closest);

            // Set active scripts according to closest station
            if (closest == "Customer")
                Active(false, true, false, false);
            else if (closest == "Dispenser")
            {
                // If holding box, allow use of storage
                if (player.handleStorage.holdingBox)
                    Active(true, false, true, false);
                else if (player.handleDrink.holdingDrink)
                    Active(false, true, false, false);
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