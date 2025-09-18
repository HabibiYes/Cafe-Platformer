using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(EnterBusinessHandler))]
[RequireComponent(typeof(HandleDrink))]
[RequireComponent(typeof(HandleDispenser))]
[RequireComponent(typeof(HandleStorage))]
[RequireComponent(typeof(TrashItem))]
[RequireComponent(typeof(StationPriority))]
[RequireComponent(typeof(HandleInventory))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public Camera cam;
    public PlayerControls controls;

    // Components
    public Transform playerModel;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CinemachineInputAxisController cameraInputController;

    // Scripts
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Jumping jumping;
    [HideInInspector] public PlayerRotation playerRotation;
    [HideInInspector] public HandleDrink handleDrink;
    [HideInInspector] public HandleDispenser handleDispenser;
    [HideInInspector] public HandleStorage handleStorage;
    [HideInInspector] public TrashItem trashItem;
    [HideInInspector] public StationPriority stationPriority;

    public enum Mode
    {
        Platformer,
        Business
    }
    public Mode mode = Mode.Platformer;

    public float interactRange = 3f;

    [HideInInspector] public bool canMove = true;

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
        cameraInputController = GetComponentInChildren<CinemachineInputAxisController>();

        // Get scripts
        playerMovement = GetComponent<PlayerMovement>();
        jumping = GetComponent<Jumping>();
        playerRotation = GetComponent<PlayerRotation>();
        handleDrink = GetComponent<HandleDrink>();
        handleDispenser = GetComponent<HandleDispenser>();
        handleStorage = GetComponent<HandleStorage>();
        trashItem = GetComponent<TrashItem>();
        stationPriority = GetComponent<StationPriority>();

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

        // Lock cursor
        MouseLock.Lock();
    }

    public void Scale(GameObject go)
    {
        Vector3 scale = go.transform.localScale;
        go.transform.localScale = new Vector3(scale.x / playerModel.localScale.x, scale.y / playerModel.localScale.y, scale.z / playerModel.localScale.z);
    }

    private void OnEnable()
    {
        // Enable player controls
        controls.Player.Enable();

        // Add cursor lock & unlock
        controls.Player.LockCursor.performed += (context) => MouseLock.Lock();
        controls.Player.UnlockCursor.performed += (context) => MouseLock.Unlock();

        // Add enable and disable camera control
        MouseLock.mouseLocked += () => EnableCameraControls();
        MouseLock.mouseUnlocked += () => DisableCameraControls();
    }

    private void OnDisable()
    {
        // Disable player controls
        controls.Player.Disable();

        // Remove cursor lock & unlock
        controls.Player.LockCursor.performed -= (context) => MouseLock.Lock();
        controls.Player.UnlockCursor.performed -= (context) => MouseLock.Unlock();

        // Remove enable and disable camera control
        MouseLock.mouseLocked -= () => EnableCameraControls();
        MouseLock.mouseUnlocked -= () => DisableCameraControls();
    }

    private void EnableCameraControls()
    {
        cameraInputController.Controllers[0].Enabled = true;
        cameraInputController.Controllers[1].Enabled = true;
    }

    private void DisableCameraControls()
    {
        cameraInputController.Controllers[0].Enabled = false;
        cameraInputController.Controllers[1].Enabled = false;
    }

    private void BusinessMode()
    {
        // Enable business scripts
        handleDrink.enabled = true;
        handleDispenser.enabled = true;
        handleStorage.enabled = true;
        trashItem.enabled = true;

        mode = Mode.Business;
    }

    private void PlatformerMode()
    {
        // Disable business scripts
        handleDrink.enabled = false;
        handleDispenser.enabled = false;
        handleStorage.enabled = false;
        trashItem.enabled = false;

        mode = Mode.Platformer;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    
    public void DisableMovement()
    {
        canMove = false;
    }
}
