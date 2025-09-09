using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BusinessSpots))]
public class Business : MonoBehaviour
{
    [HideInInspector] public BusinessSpots businessSpots;

    private void Awake()
    {
        businessSpots = GetComponent<BusinessSpots>();
        businessSpots.CreateSpots();
    }
}
