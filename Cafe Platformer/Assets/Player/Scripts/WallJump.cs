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

    [HideInInspector] public bool canHoldWall = true;

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

        // Remove gravity and start slipping
        player.rb.useGravity = false;
        player.rb.linearVelocity = new Vector3(0, -slipSpeed, 0);

        // Set rotation
        player.playerRotation.SetRotation(Quaternion.LookRotation(-wallNormal));

        // Start wall connect animation
        player.animator.SetTrigger("WallConnect");
        player.animator.ResetTrigger("WallSlideFinished");

        Debug.Log("Connected to wall");
    }

    private void ReleaseFromWall()
    {
        onWall = false;
        player.playerMovement.enabled = true;

        player.rb.useGravity = true;

        player.animator.ResetTrigger("WallConnect");

        Debug.Log("Released from wall");
    }

    private void JumpFromWall()
    {
        ReleaseFromWall();

        player.rb.AddForce(wallNormal * horizontalWallJumpForce + Vector3.up * verticalWallJumpForce, ForceMode.Impulse);

        // Rotate player away from wall
        player.playerRotation.SetRotation(Quaternion.LookRotation(wallNormal));

        player.playerMovement.TriggerJumpAnimation();

        Debug.Log("Jumped from wall");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canHoldWall)
            return;

        if (collision.collider.CompareTag("Ground"))
        {
            if (!onWall)
            {
                // Don't connect to wall if conditions are not satisfied
                wallNormal = collision.GetContact(0).normal;
                if (wallNormal.y != 0 || player.playerMovement.IsGrounded() || player.swim.isSwimming)
                    return;

                // Connect to wall
                ConnectToWall();
            }
            else
            {
                player.animator.SetTrigger("WallSlideFinished");
                ReleaseFromWall();
            }
        }
    }

    public void EnableWallHold()
    {
        canHoldWall = true;
    }

    public void DisableWallHold()
    {
        canHoldWall = false;
    }
}