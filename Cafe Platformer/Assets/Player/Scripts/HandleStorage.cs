using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleStorage : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;
    [SerializeField] private GameObject storageBox;

    [HideInInspector] public bool holdingBox = false;
    StorageBox box;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!holdingBox && !player.handleDrink.holdingDrink)
        {
            GetStorageBox();
        }
        else if (holdingBox)
        {
            UseStorageBox();
        }
    }

    private void GetStorageBox()
    {
        // Get storage box
        Storage storage = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.storages, range, transform.position);
        if (storage != null && player.controls.Player.Interact.WasPressedThisFrame())
        {
            Debug.Log("Found storage");

            // Storage to fill
            Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, Mathf.Infinity, transform.position);
            List<string> drinksToFill = GameData.Instance.business.drinks.Select(x =>
            {
                return dispenser.supplies.drinkSupplies[x.name] < dispenser.supplies.maxSupplies ? x.name : "";
            }).ToList();
            drinksToFill.RemoveAll(x => x == "");

            List<int> amountToFill = drinksToFill.Select(x => dispenser.supplies.maxSupplies - dispenser.supplies.drinkSupplies[x]).ToList();

            Dictionary<string, int> fill = drinksToFill.Zip(amountToFill, (key, value) => new { Key = key, Value = value }).ToDictionary(item => item.Key, item => item.Value);

            // Check if amount to fill for all is over zero
            if (fill.Values.Sum() <= 0)
                return;

            // Create box
            GameObject go = Instantiate(storageBox, transform.position + player.playerModel.forward, Quaternion.identity, player.playerModel);
            player.Scale(go);
            box = go.GetComponent<StorageBox>();
            box.storage = fill;

            holdingBox = true;
        }
    }

    private void UseStorageBox()
    {
        // Use storage box
        Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, range, transform.position);
        if (dispenser != null && player.controls.Player.Interact.WasPressedThisFrame())
        {
            dispenser.supplies.FillSupplies(box.storage);
            Destroy(box.gameObject);

            holdingBox = false;
            box = null;
        }
    }
}