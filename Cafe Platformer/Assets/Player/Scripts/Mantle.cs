using UnityEngine;

public class Mantle : MonoBehaviour
{
    Player player;

    [SerializeField] private float mantleCheckDistance = 2f;
    [SerializeField] private LayerMask groundLayer;

    Collider col;

    RaycastHit ledgeHit;

    private void Start()
    {
        player = GetComponent<Player>();

        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!player.playerMovement.IsGrounded())
        {
            if (CheckLedge())
                MantleOntoLedge();
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
        player.rb.linearVelocity = Vector3.zero;
        player.rb.MovePosition(ledgeHit.point + Vector3.up * col.bounds.extents.y);
    }
}