using UnityEngine;

public class ChooseSpot : MonoBehaviour
{
    Customer customer;

    private void Awake()
    {
        customer = GetComponent<Customer>();
    }

    public void FindClosestSpot()
    {
        customer.spot = GetObjectFromDistance.FindClosestPosition(customer.business.businessSpots.availableSpots, Mathf.Infinity, transform.position);
        customer.business.businessSpots.availableSpots.Remove(customer.spot);
    }
}