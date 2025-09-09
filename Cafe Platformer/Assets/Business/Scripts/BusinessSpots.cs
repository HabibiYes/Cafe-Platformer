using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BusinessSpots : MonoBehaviour
{
    [SerializeField] private SplineContainer spotSpline;
    [SerializeField] private int targetSpotCount = 0;
    [SerializeField] private float spacing = 0.1f;
    [SerializeField] private Align align = Align.Center;

    private enum Align { Left, Center, Right }

    [HideInInspector] public List<Vector3> spots = new();
    [HideInInspector] public List<Vector3> availableSpots = new();

    public void CreateSpots()
    {
        spots.Clear();

        int actualSpotCount = targetSpotCount;

        // Get real spot count
        for (int i = 0; i < targetSpotCount; i++)
        {
            if (i * spacing < 0f || i * spacing > 1f)
            {
                actualSpotCount -= 1;
                continue;
            }
        }

        // Create spots
        for (int i = 0; i < actualSpotCount; i++)
        {
            Vector3 spot = Vector3.zero;
            if (align == Align.Left)
                spot = spotSpline.EvaluatePosition(i * spacing);
            else if (align == Align.Center)
                spot = spotSpline.EvaluatePosition(i * spacing - ((actualSpotCount - 1) * (spacing / 2)) + 0.5f);
            else if (align == Align.Right)
                spot = spotSpline.EvaluatePosition(1 - i * spacing);
            spots.Add(spot);
        }

        // Create available spots list
        availableSpots = spots;
    }

    private void OnDrawGizmos()
    {
        if (spotSpline != null)
        {
            Gizmos.color = Color.red;

            if (!Application.isPlaying)
                CreateSpots();

            foreach (Vector3 spot in spots)
            {
                Gizmos.DrawWireSphere(spot, 0.5f);
            }
        }
    }
}
