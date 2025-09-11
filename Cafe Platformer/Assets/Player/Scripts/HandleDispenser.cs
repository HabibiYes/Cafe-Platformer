using UnityEngine;

public class HandleDispenser : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!player.handleDrink.holdingDrink)
        {
            Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, range, transform.position);

            if (player.controls.Player.DispenserPositive.WasPressedThisFrame())
                dispenser.ChangeSelectedDrink(1);
            else if (player.controls.Player.DispenserNegative.WasPressedThisFrame())
                dispenser.ChangeSelectedDrink(-1);
        }
    }
}