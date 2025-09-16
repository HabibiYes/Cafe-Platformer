using UnityEngine;

public class StationPriority : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.mode == Player.Mode.Business)
        {
            if (SelectionOverlap())
            {
                float distToDispenser = Vector3.Distance(transform.position, player.handleDispenser.dispenser.transform.position);
                float distToStorage = Vector3.Distance(transform.position, player.handleStorage.storage.transform.position);
                if (distToDispenser < distToStorage)
                {
                    player.handleDispenser.enabled = true;
                    player.handleStorage.enabled = false;
                }
                else if (distToStorage < distToDispenser)
                {
                    player.handleDispenser.enabled = false;
                    player.handleStorage.enabled = true;
                }
            }
            else
            {
                player.handleDispenser.enabled = true;
                player.handleStorage.enabled = true;
            }
        }
    }

    private bool SelectionOverlap()
    {
        return player.handleDispenser.dispenser != null && player.handleStorage.storage != null;
    }
}