using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChooseSpot))]
[RequireComponent(typeof(OrderDrink))]
public class Customer : MonoBehaviour
{
    [HideInInspector] public Vector3 spot;

    [HideInInspector] public NavMeshAgent agent;

    private ChooseSpot chooseSpot;
    private OrderDrink orderDrink;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Get components
        chooseSpot = GetComponent<ChooseSpot>();
        orderDrink = GetComponent<OrderDrink>();
    }

    private void Start()
    {
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
}
