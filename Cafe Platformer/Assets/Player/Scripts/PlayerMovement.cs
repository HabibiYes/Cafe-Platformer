using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    Rigidbody rb;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundCheckDistance = 1f;

    [Header("Movement")]
    [SerializeField] private float walkAcceleration = 20f;
    [SerializeField] private float sprintAcceleration = 70f;
    [SerializeField] private float drag = 10f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float airSpeedMultiplier = 0.25f;

    // Scaled acceleration
    private float acceleration { get => player.controls.Player.Sprint.IsPressed() ? sprintAcceleration : walkAcceleration; }

    [HideInInspector] public Vector3 moveDir;

    [Header("Slopes")]
    [SerializeField] private float maxSlopeAngle = 40f;
    [SerializeField] private float slopeLerpSpeed = 7.5f;
    [SerializeField] private float slipForce = 10f;
    RaycastHit slopeHit;

    [Header("Animation")]
    [SerializeField] private float animationLerpSpeed = 10f;
    private AnimationCurve movementAnimationCurve = new();

    private void Awake()
    {
        // Get base player component
        player = GetComponent<Player>();

        // Get rigidbody component
        rb = GetComponent<Rigidbody>();

        // Add animation curve keys
        movementAnimationCurve.AddKey(0f, 0f);
        movementAnimationCurve.AddKey(walkAcceleration / drag / maxSpeed, 0.5f);
        movementAnimationCurve.AddKey(1f, 1f);
    }

    private void Update()
    {
        moveDir = GetMoveDirection();
    }

    private void FixedUpdate()
    {
        // Add player force
        MovePlayer();

        ApplyDrag();

        SpeedLimit();

        // Set movement blend based on current speed, and lerp
        player.animator.SetFloat("Blend", Mathf.Lerp(player.animator.GetFloat("Blend"), movementAnimationCurve.Evaluate(rb.linearVelocity.magnitude / maxSpeed), animationLerpSpeed * Time.deltaTime));

        if (OnValidSlope())
        {
            // Attach the player to the slope
            KeepPlayerOnSlope();
        }

        if (OnInvalidSlope())
        {
            // Slip down slope
            Slip();
        }
    }

    public Vector3 GetMoveDirection(bool allowVertical = false)
    {
        // Get input
        Vector2 input = player.controls.Player.Move.ReadValue<Vector2>();

        // Get camera forward and right vectors
        Vector3 forward = player.cam.transform.forward;
        Vector3 right = player.cam.transform.right;
        if (!allowVertical)
        {
            forward.y = 0;
            right.y = 0;
        }

        // Multiply camera vectors by input
        Vector3 forwardRelative = forward * input.y;
        Vector3 rightRelative = right * input.x;

        // Get movement direction
        if (player.canMove)
            return (forwardRelative + rightRelative).normalized;
        else
            return Vector3.zero;
    }

    private void MovePlayer()
    {
        // Add movement force
        if (OnValidSlope())
            rb.AddForce(GetSlopeMoveDirection() * acceleration, ForceMode.Force);
        else if (IsGrounded())
            rb.AddForce(moveDir * acceleration, ForceMode.Force);
        else
            rb.AddForce(moveDir * acceleration * airSpeedMultiplier, ForceMode.Force);

        rb.useGravity = !OnValidSlope();
    }

    private void SpeedLimit()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            // Limit speed to max speed then reapply Y velocity
            float yVelocity = rb.linearVelocity.y;
            rb.linearVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, yVelocity, rb.linearVelocity.z);
        }
    }

    private void ApplyDrag()
    {
        // Apply drag
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (OnValidSlope())
            rb.AddForce(-rb.linearVelocity * drag, ForceMode.Force);
        else if (IsGrounded())
            rb.AddForce(-flatVelocity * drag, ForceMode.Force);
        else
            rb.AddForce(-flatVelocity * drag * airSpeedMultiplier, ForceMode.Force);
    }

    private void KeepPlayerOnSlope()
    {
        // Lerp the player towards the spherecast center, keeping them attached to the slope
        rb.position = Vector3.Lerp(rb.position, SphereCastCenter(transform.position, Vector3.down, slopeHit.distance) + Vector3.up * 0.5f, slopeLerpSpeed * Time.deltaTime);
    }

    private void Slip()
    {
        rb.AddForce(Vector3.ProjectOnPlane(Vector3.down * slipForce, slopeHit.normal), ForceMode.Force);
    }

    private Vector3 SphereCastCenter(Vector3 origin, Vector3 direction, float distance)
    {
        return origin + (direction.normalized * distance);
    }

    public bool IsGrounded(bool checkJumping = true)
    {
        if (checkJumping && player.jumping.isJumping)
            return false;

        return Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, ground);
    }

    public bool OnValidSlope(bool checkJumping = true)
    {
        if (checkJumping && player.jumping.isJumping)
            return false;

        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out slopeHit, groundCheckDistance, ground))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0 && angle <= maxSlopeAngle;
        }
        return false;
    }

    private bool OnInvalidSlope(bool checkJumping = true)
    {
        if (checkJumping && player.jumping.isJumping)
            return false;

        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out slopeHit, groundCheckDistance, ground) && !player.jumping.isJumping)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0 && angle > maxSlopeAngle;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(transform.position, Vector3.down * groundCheckDistance));
    }
}
