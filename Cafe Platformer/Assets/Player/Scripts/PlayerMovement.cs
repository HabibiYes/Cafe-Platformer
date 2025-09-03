using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    Rigidbody rb;

    [SerializeField] private LayerMask ground;

    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float drag = 0.7f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float airSpeedMultiplier = 0.25f;

    Vector3 moveDir;

    private void Awake()
    {
        // Get base player component
        player = GetComponent<Player>();

        // Get rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        // Add player force
        MovePlayer();

        // Limit player speed
        SpeedLimit();
    }

    private void MyInput()
    {
        // Get input
        Vector2 input = player.controls.Player.Move.ReadValue<Vector2>();

        // Get camera forward and right vectors
        Vector3 forward = player.cam.transform.forward;
        Vector3 right = player.cam.transform.right;
        forward.y = 0;
        right.y = 0;

        // Multiply camera vectors by input
        Vector3 forwardRelative = forward * input.y;
        Vector3 rightRelative = right * input.x;

        // Get movement direction
        moveDir = (forwardRelative + rightRelative).normalized;
    }

    private void MovePlayer()
    {
        // Add movement force and horizontal drag
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (IsGrounded())
        {
            rb.AddForce(moveDir * acceleration, ForceMode.Force);
            rb.AddForce(-flatVelocity * drag, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDir * acceleration * airSpeedMultiplier, ForceMode.Force);
            rb.AddForce(-flatVelocity * drag * airSpeedMultiplier, ForceMode.Force);
        }
    }

    private void SpeedLimit()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            // Get current Y velocity
            float yVelocity = rb.linearVelocity.y;

            // Limit speed to max speed
            rb.linearVelocity = flatVelocity.normalized * maxSpeed;

            // Reapply Y velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, yVelocity, rb.linearVelocity.z);
        }
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, 1f, ground);
    }
}
