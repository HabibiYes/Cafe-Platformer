using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(EnterBusinessHandler))]
[RequireComponent(typeof(GetDrink))]
[RequireComponent(typeof(GiveDrink))]
[RequireComponent(typeof(HandleDrink))]
[RequireComponent(typeof(HandleDispenser))]
[RequireComponent(typeof(TrashItem))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public Camera cam;
    public PlayerControls controls;

    // Components
    public Transform playerModel;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;

    // Scripts
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Jumping jumping;
    [HideInInspector] public PlayerRotation playerRotation;
    [HideInInspector] public HandleDrink handleDrink;

    private void Awake()
    {
        // Set instance and keep loaded, or destroy if already existing
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // Create new controls
        controls = new();

        // Get Rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        // Get scripts
        playerMovement = GetComponent<PlayerMovement>();
        jumping = GetComponent<Jumping>();
        playerRotation = GetComponent<PlayerRotation>();
        handleDrink = GetComponent<HandleDrink>();
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
