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

    Storage storage;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Get closest storage
        storage = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.storages, range, transform.position);

        CycleStorage();
        if (!holdingBox && !player.handleDrink.holdingDrink)
        {
            GetStorageBox();
        }
        else if (holdingBox)
        {
            if (storage != null)
                AddStorageToBox();
            else
                UseStorageBox();
        }
    }

    private void CycleStorage()
    {
        if (storage == null)
            return;

        // Cycle through storage drink options
        if (player.controls.Player.CyclePositive.WasPressedThisFrame())
        {
            storage.ChangeSelectedDrink(1);
        }
        else if (player.controls.Player.CycleNegative.WasPressedThisFrame())
        {
            storage.ChangeSelectedDrink(-1);
        }
    }

    private void GetStorageBox()
    {
        if (storage == null || !player.controls.Player.Interact.WasPressedThisFrame())
            return;

        // Get storage box
        Debug.Log("Found storage");

        // Create box
        GameObject go = Instantiate(storageBox, transform.position + player.playerModel.forward, Quaternion.identity, player.playerModel);
        player.Scale(go);
        box = go.GetComponent<StorageBox>();

        holdingBox = true;
    }

    private void AddStorageToBox()
    {
        if (!player.controls.Player.Interact.WasPressedThisFrame() || !storage.HasStorage())
            return;

        // Check if box storage already contains key. If true, it adds, else, updates by 1
        if (!box.storage.TryAdd(storage.storageSelectedDrink.name, 1))
            box.storage[storage.storageSelectedDrink.name] += 1;

        // Remove drink from storage
        storage.RemoveStorage(new Dictionary<string, int>() { [storage.storageSelectedDrink.name] = 1 });

        Debug.Log($"Added 1 {storage.storageSelectedDrink.name} to storage box");
    }

    private void UseStorageBox()
    {
        Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, range, transform.position);
        if (dispenser == null || !player.controls.Player.Interact.WasPressedThisFrame())
            return;

        // Use storage box
        dispenser.supplies.FillSupplies(box.storage);
        Destroy(box.gameObject);

        holdingBox = false;
        box = null;
    }
}