using UnityEngine;

public class TrashItem : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Find closest trash can
        TrashCan trashCan = GetObjectFromDistance.FindClosestObject(GameData.Instance.business.trashCans, player.interactRange, transform.position);

        if (trashCan != null && player.handleDrink.holdingDrink && player.controls.Player.Interact.WasPressedThisFrame())
        {
            if (trashCan.AddTrash())
            {
                player.handleDrink.ResetDrink();
                Debug.Log("Trashed item");
            }
        }
    }
}