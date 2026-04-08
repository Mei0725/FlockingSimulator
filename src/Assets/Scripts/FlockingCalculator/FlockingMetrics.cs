using System.Collections.Generic;
using UnityEngine;

public static class FlockingMetrics
{
    public static float CalMeanSpeed(List<Vector3> velos)
    {
        float speed = 0;

        foreach (Vector3 velo in velos)
        {
            speed += velo.magnitude;
        }

        return speed / velos.Count;
    }

    public static float CalAlignMetric(List<Vector3> velos)
    {
        Vector3 result = Vector3.zero;

        foreach (Vector3 velo in velos)
        {
            result += velo.normalized;
        }

        return result.magnitude / velos.Count;
    }

    public static Vector3 CalCenter(List<Vector3> positions)
    {
        Vector3 result = Vector3.zero;

        foreach (Vector3 position in positions)
        {
            result += position;
        }

        return result / positions.Count;
    }

    public static float CalCohesion(List<Vector3> positions)
    {
        float cohesion = 0;
        Vector3 center = CalCenter(positions);

        foreach (Vector3 position in positions)
        {
            cohesion += (position - center).magnitude;
        }

        return cohesion / positions.Count;
    }

    public static float CalCohesion(Vector3 center, List<Vector3> positions)
    {
        float cohesion = 0;

        foreach (Vector3 position in positions)
        {
            cohesion += (position - center).magnitude;
        }

        return cohesion / positions.Count;
    }
}
