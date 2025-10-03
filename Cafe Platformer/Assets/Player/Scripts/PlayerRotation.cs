using System.Collections;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    Player player;

    private void Start()
    {
        // Get base player
        player = GetComponent<Player>();
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

    public void ResetTilt()
    {
        SetRotation(Quaternion.Euler(new Vector3(0, player.playerModel.rotation.eulerAngles.y, player.playerModel.rotation.eulerAngles.z)));
    }

    public void ResetRoll()
    {
        SetRotation(Quaternion.Euler(new Vector3(player.playerModel.rotation.eulerAngles.x, player.playerModel.rotation.eulerAngles.y, 0)));
    }
}