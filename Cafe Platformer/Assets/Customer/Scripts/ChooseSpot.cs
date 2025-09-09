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
        customer.spot = GetObjectFromDistance.FindClosestPosition(business.spots, Mathf.Infinity, transform.position);
    }
}