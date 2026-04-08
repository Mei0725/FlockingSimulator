using FlockingEntity;
using System.Collections.Generic;
using UnityEngine;

public static class Force
{
    private static float vMax = 14.0f;
    private static float perception = 5.0f;
    private static float obsAviodRatioCor = 10.0f;
    private static float obsAviodCenterCor = 0.1f;
    private static float aviodAccMax = 200.0f;
    private static float sepaDisCor = 0.4f;

    public static Vector3 CalForce(int i, List<Vector3> ps, List<Vector3> vs, FlockingParameter info, Vector3 goal)
    {
        Vector3 alignForce = calAlignForce(i, vs);
        Vector3 separationForce = calSeparationForce(i, ps);
        Vector3 cohesionForce = calCohesionForce(i, ps);
        Vector3 obsAvoidForce = calObsAvoidForce(ps[i], info.obs);
        Vector3 goalForce = calGoalSeekingForce(ps[i], goal, vs[i]);
        //return alignForce.normalized * info.ka + separationForce.normalized * info.ks + cohesionForce.normalized * info.kc + obsAvoidForce.normalized * info.ko + goalForce.normalized * info.kg;
        return alignForce * info.ka + separationForce * info.ks + cohesionForce * info.kc + obsAvoidForce * info.ko + goalForce * info.kg;
    }

    public static Vector3 calAlignForce(int i, List<Vector3> vs)
    {
        Vector3 avgVelo = new Vector3(0, 0, 0);
        if (vs.Count <= 1)
        {
            return avgVelo;
        }

        for (int j = 0; j < vs.Count; j++)
        {
            if (j == i)
                continue;

            avgVelo += vs[j];
        }
        avgVelo /= (vs.Count - 1);
        return avgVelo - vs[i];
    }

    public static Vector3 calCohesionForce(int i, List<Vector3> ps)
    {
        if (ps.Count <= 1)
        {
            return Vector3.zero;
        }

        Vector3 center = Vector3.zero;
        for (int j = 0; j < ps.Count; j++)
        {
            if (j == i)
                continue;

            center += ps[j];
        }
        center /= (ps.Count - 1);
        return center - ps[i];
    }

    public static Vector3 calSeparationForce(int i, List<Vector3> ps)
    {
        if (ps.Count <= 1)
        {
            return Vector3.zero;
        }

        Vector3 force = Vector3.zero;
        for (int j = 0; j < ps.Count; j++)
        {
            if (j == i)
                continue;

            Vector3 diff = ps[i] - ps[j];
            float sqrDist = diff.sqrMagnitude;
            //force += diff / sqrDist;
            if (sqrDist <= sepaDisCor)
            {
                force += diff * 5.0f / sqrDist;
            } else
            {
                force += diff / sqrDist;
            }
        }
        return force;
    }

    public static Vector3 calObsAvoidForce(Vector3 p, List<Obstacle> obs)
    {
        Vector3 totalForce = Vector3.zero;

        foreach (var o in obs)
        {
            Vector3 diff = p - o.p.ToVector3();
            float dist = diff.magnitude;

            float sensingRadius = o.r + perception;

            if (dist < sensingRadius && dist > 0)
            {
                Vector3 dir = diff.normalized;
                float forceMag = 1f / (dist - o.r + obsAviodCenterCor) - 1f / sensingRadius;
                totalForce += dir * forceMag * obsAviodRatioCor;
            }

            if (dist <= o.r)
            {
                Vector3 dir = diff.normalized;
                totalForce += dir * aviodAccMax;
            }
        }
        if (totalForce.magnitude > aviodAccMax)
        {
            return totalForce.normalized * aviodAccMax;
        }
        return totalForce;
    }

    public static Vector3 calGoalSeekingForce(Vector3 p, Vector3 goal, Vector3 v)
    {
        Vector3 direction = goal - p;

        if (direction.sqrMagnitude < 0)
            return Vector3.zero;
        // easiest way, but not stable
        //return direction.normalized;

        // the first choose
        //direction = direction.normalized;

        //Vector3 steering = direction - velocity;

        //if (steering.magnitude > 10)
        //    steering = steering.normalized * 10;

        //return steering;

        // the second choose
        direction = direction.normalized;
        return direction * vMax - v;

    }
}
