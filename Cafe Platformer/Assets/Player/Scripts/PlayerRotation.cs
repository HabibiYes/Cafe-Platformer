using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    Player player;

    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        // Get base player
        player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        if (player.playerMovement.moveDir.magnitude > 0)
            player.playerModel.rotation = Quaternion.Lerp(player.playerModel.rotation, Quaternion.LookRotation(player.playerMovement.moveDir), rotationSpeed * Time.deltaTime);
    }
}