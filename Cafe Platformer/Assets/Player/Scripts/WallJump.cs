using UnityEngine;

public class WallJump : MonoBehaviour
{
    Player player;

    [Header("On Wall")]
    [SerializeField] private float slipSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float horizontalWallJumpForce = 10f;
    [SerializeField] private float verticalWallJumpForce = 5f;

    [HideInInspector] public bool onWall = false;
    Vector3 wallNormal;

    private void Start()
    {
        player = GetComponent<Player>();

        // Jump from wall binded to jump
        player.controls.Player.Jump.performed += (context) => { if (onWall) JumpFromWall(); };
    }

    private void ConnectToWall()
    {
        onWall = true;
        player.playerMovement.enabled = false;
        player.jumping.enabled = false;

        // Remove gravity and start slipping
        player.rb.useGravity = false;
        player.rb.linearVelocity = new Vector3(0, -slipSpeed, 0);

        Debug.Log("Connected to wall");
    }

    private void ReleaseFromWall()
    {
        onWall = false;
        player.playerMovement.enabled = true;
        player.jumping.enabled = true;

        player.rb.useGravity = true;

        Debug.Log("Released from wall");
    }

    private void JumpFromWall()
    {
        ReleaseFromWall();

        player.rb.AddForce(wallNormal * horizontalWallJumpForce + Vector3.up * verticalWallJumpForce, ForceMode.Impulse);

        Debug.Log("Jumped from wall");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            // Check if hit surface is a valid wall and the player if off the ground
            wallNormal = collision.GetContact(0).normal;
            if (wallNormal.y != 0 || player.playerMovement.IsGrounded())
                return;

            // Connect to wall
            ConnectToWall();
        }
    }
}