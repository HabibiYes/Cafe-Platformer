using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawning : MonoBehaviour
{
    Business business;

    [SerializeField] private GameObject customer;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 10f;

    private List<Transform> spawnPoints = new();

    float time = 0f;
    float nextSpawnTime = 0f;

    private void Start()
    {
        // Get base business
        business = GetComponent<Business>();
        
        // Get all spawn points
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform).ToList();
    }

    private void Update()
    {
        // Update current time, if it is over spawn time, spawn new customer
        time += Time.deltaTime;
        if (time >= nextSpawnTime)
        {
            SpawnCustomer();
            SetNewSpawnTime();
        }
    }

    private void SetNewSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        time = 0f;
    }

    private void SpawnCustomer()
    {
        if (spawnPoints.Count == 0 || business.businessSpots.availableSpots.Count == 0)
            return;
        Instantiate(customer, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
    }
}