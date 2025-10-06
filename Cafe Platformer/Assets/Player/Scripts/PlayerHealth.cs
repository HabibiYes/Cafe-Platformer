using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Player player;

    [Header("Settings")]
    public float maxHealth = 25f;
    public float health;
    public int roundedHealth { get => Mathf.FloorToInt(health); }

    [Header("Regeneration")]
    [SerializeField, Tooltip("Time to wait before starting regeneration")] private float regenerationStartTime = 30f;
    [SerializeField, Tooltip("Health regen rate in health per second")] private float regenerationRate = 1f;
    float regenerationTimer;
    bool regenerating = false;

    Coroutine regenerationCoroutine;

    public delegate void PlayerDeath();
    public PlayerDeath onPlayerDeath;

    private void Start()
    {
        player = GetComponent<Player>();

        player.controls.Player.Interact.performed += (context) => Heal(10);
        player.controls.Player.AltInteract.performed += (context) => Damage(10);

        MaxHealth();
    }

    private void Update()
    {
        if (health < maxHealth)
        {
            regenerationTimer += Time.deltaTime;

            if (regenerationTimer >= regenerationStartTime && !regenerating)
            {
                regenerationCoroutine = StartCoroutine(Regenerate());
            }
        }
    }

    public void MaxHealth()
    {
        health = maxHealth;

        Debug.Log("Health maxed");
    }

    public void Damage(float damage)
    {
        // Stop regenerating on damage
        if (regenerationCoroutine != null)
            StopCoroutine(regenerationCoroutine);

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        // Call player death
        if (health == 0)
        {
            onPlayerDeath?.Invoke();
        }

        Debug.Log("Damaged for " + damage + " health");
    }

    public void Heal(float heal)
    {
        health = Mathf.Clamp(health + heal, 0, maxHealth);

        Debug.Log("Healed for " + heal + " health");
    }

    public void ResetRegenerationTimer()
    {
        regenerationTimer = 0f;

        Debug.Log("Regeneration timer reset");
    }

    private IEnumerator Regenerate()
    {
        regenerating = true;

        Debug.Log("Started regenerating");

        float lastTime = Time.time - 1f;

        while (health < maxHealth)
        {
            // Add rate health every second
            Heal(regenerationRate * (Time.time - lastTime));
            lastTime = Time.time;
            yield return new WaitForSeconds(1f);
        }

        regenerating = false;
        regenerationCoroutine = null;
        ResetRegenerationTimer();
    }
}