using System;
using System.Collections.Generic;
using UnityEngine;

public static class GetObjectFromDistance
{
    public static T FindClosestObject<T>(List<T> list, float maxDistance, Vector3 position, Func<T, bool> check = null) where T : MonoBehaviour
    {
        // Set check func if null
        if (check == null)
            check = (val) => true;

        // Find closest object based on distance and custom check function
        float closest = Mathf.Infinity;
        T closestObj = null;

        foreach (T obj in list)
        {
            float dist = Vector3.Distance(position, obj.transform.position);
            if (dist < closest && dist < maxDistance && check(obj))
            {
                closest = dist;
                closestObj = obj;
            }
        }

        return closestObj;
    }

    public static Vector3 FindClosestPosition(List<Vector3> list, float maxDistance, Vector3 position)
    {
        float closest = Mathf.Infinity;
        Vector3 closestPosition = Vector3.zero;

        foreach (Vector3 pos in list)
        {
            float dist = Vector3.Distance(position, pos);
            if (dist < closest && dist < maxDistance)
            {
                closest = dist;
                closestPosition = pos;
            }
        }

        return closestPosition;
    }
}