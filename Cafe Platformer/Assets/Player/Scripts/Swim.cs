using UnityEngine;

public class Swim : MonoBehaviour
{
    Player player;

    [Header("Settings")]
    [SerializeField] private float swimSpeed = 8f;
    [SerializeField] private float drag = 15f;
    [SerializeField] private float maxSpeed = 10f;

    [HideInInspector] public bool isSwimming = false;

    Vector3 moveDir;

    private void Start()
    {
        player = GetComponent<Player>();

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
    }

    private void MovePlayer()
    {
        player.rb.AddForce(moveDir * swimSpeed, ForceMode.Force);
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

    private void StartSwimming()
    {
        this.enabled = true;
        isSwimming = true;

        player.rb.useGravity = false;
        player.playerMovement.enabled = false;

        Debug.Log("Started swimming");
    }

    private void StopSwimming()
    {
        this.enabled = false;
        isSwimming = false;

        player.rb.useGravity = true;
        player.playerMovement.enabled = true;

        Debug.Log("Stopped swimming");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StartSwimming();
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