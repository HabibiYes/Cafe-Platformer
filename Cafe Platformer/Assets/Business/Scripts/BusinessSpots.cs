using System.Collections.Generic;
using UnityEngine;

public class BusinessSpots : MonoBehaviour
{
    [SerializeField] private Transform spotOrigin;
    [SerializeField] private float spacing = 1.5f;

    [HideInInspector] public List<Vector3> spots = new();

    private void CreateSpots()
    {
        spots.Clear();

        // Get collider bounds if available
        float width;
        if (TryGetComponent(out Collider collider))
        {
            width = collider.bounds.size.x;
        }
        else
            return;

        int spotCount = Mathf.FloorToInt(width / spacing);

        // Create spots
        for (int i = 0; i < spotCount; i++)
        {
            Vector3 spot = spotOrigin.position;
            spot.x = i * spacing - ((spotCount - 1) * (spacing / 2));
            spots.Add(spot);
        }
    }

    private void Start()
    {
        CreateSpots();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (!Application.isPlaying)
            CreateSpots();

        foreach (Vector3 spot in spots)
            Gizmos.DrawWireSphere(spot, 0.5f);
    }
}
