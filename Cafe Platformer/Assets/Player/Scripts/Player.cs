using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(EnterBusinessHandler))]
public class Player : MonoBehaviour
{
    public Camera cam;
    public PlayerControls controls;

    // Components
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;

    // Scripts
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Jumping jumping;
    [HideInInspector] public PlayerRotation playerRotation;

    private void Awake()
    {
        // Create new controls
        controls = new();

        // Get Rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        // Get scripts
        playerMovement = GetComponent<PlayerMovement>();
        jumping = GetComponent<Jumping>();
        playerRotation = GetComponent<PlayerRotation>();

        // Keep loaded
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        // Enable player controls
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        // Disable player controls
        controls.Player.Disable();
    }
}
