using UnityEngine;

public class ChooseSpot : MonoBehaviour
{
    Customer customer;

    Business business;

    private void Awake()
    {
        customer = GetComponent<Customer>();
    }

    private void Start()
    {
        // Get business
        business = GameObject.FindFirstObjectByType<Business>();
        Debug.Log(business == null);
    }

    public void FindClosestSpot()
    {
        // Check distance to each spot
        float closest = Mathf.Infinity;
        Vector3 closestSpot = Vector3.zero;

        foreach (Vector3 spot in business.spots)
        {
            float distance = Vector3.Distance(transform.position, spot);
            if (distance < closest)
            {
                closestSpot = spot;
                closest = distance;
            }
        }

        // Set spot to found spot
        customer.spot = closestSpot;
    }
}