using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(WallJump))]
[RequireComponent(typeof(Swim))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(EnterBusinessHandler))]
[RequireComponent(typeof(HandleDrink))]
[RequireComponent(typeof(HandleDispenser))]
[RequireComponent(typeof(HandleStorage))]
[RequireComponent(typeof(TrashItem))]
[RequireComponent(typeof(StationPriority))]
[RequireComponent(typeof(HandleInventory))]
[RequireComponent(typeof(HandleClothing))]
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
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public WallJump wallJump;
    [HideInInspector] public Swim swim;
    [HideInInspector] public PlayerRotation playerRotation;
    [HideInInspector] public HandleDrink handleDrink;
    [HideInInspector] public HandleDispenser handleDispenser;
    [HideInInspector] public HandleStorage handleStorage;
    [HideInInspector] public HandleInventory handleInventory;
    [HideInInspector] public HandleClothing handleClothing;
    [HideInInspector] public TrashItem trashItem;
    [HideInInspector] public StationPriority stationPriority;

    // Animation event handler
    [HideInInspector] public HandleAnimationEvents handleAnimationEvents;

    public enum Mode
    {
        Platformer,
        Business
    }
    public Mode mode = Mode.Platformer;

    public float interactRange = 3f;

    [SerializeField] private int maxMoney = 999999999;
    public int money { get; private set; }
    public string moneyDisplay
    {
        get
        {
            if (money < 1000)
                return "$" + money;
            else if (money < 1000000)
                return "$" + Mathf.Floor(money / 10f) / 100f + "k";
            else
                return "$" + Mathf.Floor(money / 10000f) / 100f + "m";
        }
    }

    public delegate void MoneyChanged();
    public MoneyChanged onMoneyChanged;

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
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        wallJump = GetComponent<WallJump>();
        swim = GetComponent<Swim>();
        playerRotation = GetComponent<PlayerRotation>();
        handleDrink = GetComponent<HandleDrink>();
        handleDispenser = GetComponent<HandleDispenser>();
        handleStorage = GetComponent<HandleStorage>();
        handleInventory = GetComponent<HandleInventory>();
        handleClothing = GetComponent<HandleClothing>();
        trashItem = GetComponent<TrashItem>();
        stationPriority = GetComponent<StationPriority>();

        handleAnimationEvents = GetComponentInChildren<HandleAnimationEvents>();

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

    private void Start()
    {
        onMoneyChanged += UpdateMoneyText;
        SetMoney(0);
    }

    private void OnEnable()
    {
        // Enable player controls
        controls.Player.Enable();

        // Add cursor lock & unlock
        controls.Player.LockCursor.performed += (context) => { if (!handleInventory.isOpen) MouseLock.Lock(); };
        controls.Player.UnlockCursor.performed += (context) => MouseLock.Unlock();

        // Add enable and disable camera control
        MouseLock.mouseLocked += () => EnableCameraControls();
        MouseLock.mouseUnlocked += () => DisableCameraControls();
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

    public void SetMoney(int amount)
    {
        money = Mathf.Clamp(amount, 0, maxMoney);
        onMoneyChanged?.Invoke();
    }

    public void AddMoney(int amount)
    {
        money = Mathf.Clamp(money + amount, 0, maxMoney);
        onMoneyChanged?.Invoke();
    }

    public void RemoveMoney(int amount)
    {
        money = Mathf.Clamp(money - amount, 0, maxMoney);
        onMoneyChanged?.Invoke();
    }

    private void UpdateMoneyText()
    {
        CanvasHandler.Instance.moneyText.text = moneyDisplay;
    }
}
