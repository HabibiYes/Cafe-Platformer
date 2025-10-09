using UnityEngine;

public class Mantle : MonoBehaviour
{
    Player player;

    [SerializeField] private float mantleCheckDistance = 2f;
    [SerializeField] private LayerMask groundLayer;

    Collider col;

    RaycastHit ledgeHit;

    bool mantling = false;

    private void Start()
    {
        player = GetComponent<Player>();

        col = GetComponent<Collider>();

        player.handleAnimationEvents.OnAnimationEventTriggered += (eventName) => { if (eventName == nameof(MantleOntoLedge)) MantleOntoLedge(); };
    }

    private void Update()
    {
        if (!player.playerMovement.IsGrounded() && !mantling)
        {
            if (CheckLedge())
            {
                // Disable scripts and rb
                player.playerMovement.enabled = false;
                player.wallJump.DisableWallHold();
                player.rb.isKinematic = true;

                // Set position
                if (Physics.Raycast(player.rb.position, player.playerModel.forward, out RaycastHit hit, mantleCheckDistance))
                {

                    player.rb.position = new Vector3(hit.point.x, ledgeHit.point.y - 0.5f, hit.point.z) + hit.normal * 0.5f;
                    player.playerRotation.SetRotation(Quaternion.LookRotation(-hit.normal));
                }

                player.ResetAllTriggers();
                player.animator.SetTrigger("Mantle");

                mantling = true;
            }
        }
    }

    private bool CheckLedge()
    {
        // Get ledge detection point
        Vector3 position = (player.rb.position + (player.playerModel.forward * mantleCheckDistance)) + (Vector3.up * (col.bounds.extents.y + 1f));
        Ray ray = new Ray(position, Vector3.down);

        return Physics.Raycast(ray, out ledgeHit, 2f, groundLayer);
    }

    private void MantleOntoLedge()
    {
        player.rb.position = ledgeHit.point + Vector3.up * col.bounds.extents.y;

        player.playerMovement.enabled = true;
        player.wallJump.EnableWallHold();
        player.playerMovement.isJumping = false;
        player.rb.isKinematic = false;

        mantling = false;
    }
}