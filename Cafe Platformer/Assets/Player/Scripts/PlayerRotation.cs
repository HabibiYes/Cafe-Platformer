using System.Collections;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    Player player;

    [SerializeField] private float rotationSpeed = 10f;

    private void Start()
    {
        // Get base player
        player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        if (player.playerMovement.moveDir.magnitude > 0)
            SetRotation(Quaternion.Lerp(player.playerModel.rotation, Quaternion.LookRotation(player.playerMovement.moveDir), rotationSpeed * Time.deltaTime));
    }

    public void SetRotation(Quaternion rotation)
    {
        player.playerModel.rotation = rotation;
    }

    public IEnumerator TurnTorwardsRotation(Quaternion rotation, float time)
    {
        Quaternion startRotation = player.playerModel.rotation;
        float currentTime = 0;
        while (currentTime < time)
        {
            SetRotation(Quaternion.Lerp(startRotation, rotation, currentTime / time));
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}