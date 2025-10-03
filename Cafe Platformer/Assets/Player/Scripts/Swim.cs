using UnityEngine;

public class Swim : MonoBehaviour
{
    Player player;

    [Header("Settings")]
    [SerializeField] private float slowSwimAccleration = 8f;
    [SerializeField] private float fastSwimAccleration = 12f;
    [SerializeField] private float drag = 15f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float yLevelJumpDifference = 2f;

    private float acceleration { get => player.controls.Player.Sprint.IsPressed() ? fastSwimAccleration : slowSwimAccleration; }

    [Header("Animation")]
    [SerializeField] private float animationLerpSpeed = 7.5f;

    [HideInInspector] public bool isSwimming = false;
    float waterYLevel;

    Vector3 moveDir;

    private void Start()
    {
        player = GetComponent<Player>();

        // Subscribe to jump control
        player.controls.Player.Jump.performed += (context) => { if (isSwimming && transform.position.y > waterYLevel - yLevelJumpDifference) Jump(); };

        this.enabled = false;
    }

    private void Update()
    {
        // Get move direction
        moveDir = player.playerMovement.GetMoveDirection(true);

        // Player forces
        MovePlayer();
        ApplyDrag();
        SpeedLimit();

        // Set swimming animation blend
        player.animator.SetFloat("SwimBlend", Mathf.Lerp(player.animator.GetFloat("SwimBlend"), player.rb.linearVelocity.magnitude / maxSpeed, animationLerpSpeed * Time.deltaTime));

        if (moveDir.magnitude > 0)
            player.playerRotation.SetRotation(Quaternion.Lerp(player.playerModel.rotation, Quaternion.LookRotation(moveDir), rotationSpeed * Time.deltaTime));
    }

    private void MovePlayer()
    {
        player.rb.AddForce(moveDir * acceleration, ForceMode.Force);
    }

    private void ApplyDrag()
    {
        player.rb.AddForce(-player.rb.linearVelocity * drag, ForceMode.Force);
    }

    private void SpeedLimit()
    {
        if (player.rb.linearVelocity.magnitude > maxSpeed)
        {
            player.rb.linearVelocity = player.rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void Jump()
    {
        StopSwimming();

        player.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        player.playerRotation.ResetTilt();
        player.playerRotation.ResetRoll();

        player.jumping.TriggerJumpAnimation();
    }

    private void StartSwimming()
    {
        this.enabled = true;
        isSwimming = true;

        player.rb.useGravity = false;
        player.playerMovement.enabled = false;
        player.jumping.enabled = false;

        // Start swim animation
        player.animator.SetBool("Swimming", true);

        Debug.Log("Started swimming");
    }

    private void StopSwimming()
    {
        this.enabled = false;
        isSwimming = false;

        player.rb.useGravity = true;
        player.playerMovement.enabled = true;
        player.jumping.enabled = true;

        // Stop swim animation
        player.animator.SetBool("Swimming", false);

        Debug.Log("Stopped swimming");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StartSwimming();
            waterYLevel = other.bounds.center.y + other.bounds.extents.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StopSwimming();
        }
    }
}