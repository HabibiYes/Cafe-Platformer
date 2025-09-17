using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChooseSpot))]
[RequireComponent(typeof(OrderDrink))]
public class Customer : Station
{
    [HideInInspector] public Vector3 spot;
    [HideInInspector] public Business business;

    [HideInInspector] public NavMeshAgent agent;

    private ChooseSpot chooseSpot;
    public OrderDrink orderDrink;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Get components
        chooseSpot = GetComponent<ChooseSpot>();
        orderDrink = GetComponent<OrderDrink>();
    }

    private void Start()
    {
        GameData.Instance.business.AddCustomer(this);

        // Get business
        business = GameObject.FindFirstObjectByType<Business>();

        // Find spot and set destination to it
        chooseSpot.FindClosestSpot();
        agent.SetDestination(spot);
    }

    private void Update()
    {
        // Check distance to spot and order if close
        if (agent.remainingDistance < 0.1f && orderDrink.orderedDrink == null)
        {
            orderDrink.Order();
            Debug.Log(orderDrink.orderedDrink.name);
        }
    }

    public void Remove()
    {
        business.businessSpots.availableSpots.Add(spot);
        GameData.Instance.business.RemoveCustomer(this);
        Destroy(this.gameObject);
    }
}
