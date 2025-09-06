using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChooseSpot))]
public class Customer : MonoBehaviour
{
    [HideInInspector] public Vector3 spot;

    [HideInInspector] public NavMeshAgent agent;

    private ChooseSpot chooseSpot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        chooseSpot = GetComponent<ChooseSpot>();
    }

    private void Start()
    {
        // Find spot and set destination to it
        chooseSpot.FindClosestSpot();
        agent.SetDestination(spot);
    }
}
