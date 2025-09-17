using UnityEngine;

public class HandleDispenser : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!player.handleDrink.holdingDrink)
        {
            Dispenser dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, player.interactRange, transform.position);
            if (dispenser != null)
            {
                if (player.controls.Player.CyclePositive.WasPressedThisFrame())
                    dispenser.ChangeSelectedDrink(1);
                else if (player.controls.Player.CycleNegative.WasPressedThisFrame())
                    dispenser.ChangeSelectedDrink(-1);
            }
        }
    }
}