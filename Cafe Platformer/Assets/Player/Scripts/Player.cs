using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(EnterBusinessHandler))]
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
    [HideInInspector] public HandleDispenser handleDispenser;
    [HideInInspector] public TrashItem trashItem;

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
        handleDispenser = GetComponent<HandleDispenser>();
        trashItem = GetComponent<TrashItem>();

        // Enable and disable business scripts
        SceneManager.sceneLoaded += (a, b) =>
        {
            if (a.name == "Business")
            {
                BusinessMode();
            }
            else
            {
                PlatformerMode();
            }
        };
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

    private void BusinessMode()
    {
        // Enable business scripts
        handleDrink.enabled = true;
        handleDispenser.enabled = true;
        trashItem.enabled = true;
    }

    private void PlatformerMode()
    {
        // Disable business scripts
        handleDrink.enabled = false;
        handleDispenser.enabled = false;
        trashItem.enabled = false;
    }
}
