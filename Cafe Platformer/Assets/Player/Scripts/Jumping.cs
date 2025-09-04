using UnityEngine;

public class Jumping : MonoBehaviour
{
    Player player;

    [SerializeField] private float jumpForce = 7f;
    [HideInInspector] public bool isJumping = false;

    private void Awake()
    {
        // Get base player
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.controls.Player.Jump.WasPressedThisFrame() && player.playerMovement.IsGrounded())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping && player.rb.linearVelocity.y < 0)
        {
            if (player.playerMovement.OnValidSlope(false))
            {
                isJumping = false;
                player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, 0, player.rb.linearVelocity.z);

                // Trigger landing animation
                player.animator.SetTrigger("Land");
                player.animator.ResetTrigger("Jump");
            }
            else if (player.playerMovement.IsGrounded(false))
            {
                isJumping = false;

                // Trigger landing animation
                player.animator.SetTrigger("Land");
                player.animator.ResetTrigger("Jump");
            }
        }
    }

    public void Jump()
    {
        isJumping = true;
        player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.rb.linearVelocity.y + jumpForce * player.rb.mass, player.rb.linearVelocity.z);

        // Trigger jump animation
        player.animator.SetTrigger("Jump");
        player.animator.ResetTrigger("Land");
    }
}