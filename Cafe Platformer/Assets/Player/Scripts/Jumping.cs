using UnityEngine;

public class Jumping : MonoBehaviour
{
    Player player;

    [SerializeField] private float jumpForce = 7f;
    [HideInInspector] public bool isJumping = false;

    float currentYVelocity;

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
        currentYVelocity = player.rb.linearVelocity.y;
    }

    public void Jump()
    {
        player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.rb.linearVelocity.y + jumpForce * player.rb.mass, player.rb.linearVelocity.z);

        TriggerJumpAnimation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping)
        {
            if (player.playerMovement.OnValidSlope(false))
            {
                Vector3 projectedFallingVector = Vector3.ProjectOnPlane(new Vector3(0, currentYVelocity, 0), player.playerMovement.GetSlopeNormal());

                player.rb.AddForce(-projectedFallingVector, ForceMode.Impulse);

                TriggerLandAnimation();
            }
            else if (player.playerMovement.IsGrounded(false))
            {
                TriggerLandAnimation();
            }
        }
    }

    public void TriggerJumpAnimation()
    {
        isJumping = true;

        player.animator.SetTrigger("Jump");
        player.animator.ResetTrigger("Land");
    }

    public void TriggerLandAnimation()
    {
        isJumping = false;

        player.animator.SetTrigger("Land");
        player.animator.ResetTrigger("Jump");
    }
}