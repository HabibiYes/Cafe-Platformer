using UnityEngine;

public class HandleDispenser : MonoBehaviour
{
    Player player;

    [SerializeField] private float range = 3f;

    [HideInInspector] public Dispenser dispenser;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!player.handleDrink.holdingDrink)
        {
            dispenser = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.dispensers, range, transform.position);
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