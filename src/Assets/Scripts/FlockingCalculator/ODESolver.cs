using FlockingEntity;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ODESolver
{
    public static Func<int, List<Vector3>, List<Vector3>, FlockingParameter, Vector3, Vector3> ForceFunc = Force.CalForce;

    public static List<Vector3>[] EEulerMethod(List<Vector3> ps, List<Vector3> vs, FlockingParameter info, Vector3 goal)
    {
        List<Vector3> curVelos = new List<Vector3>();
        List<Vector3> curPosition = new List<Vector3>();
        //Debug.Log("start:" + curPosition.Count + ",n:" + info.n);

        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps, vs, info, goal);
            curVelos.Add(vs[i] + a * info.t);
            curPosition.Add(ps[i] + vs[i] * info.t);
        }

        return new List<Vector3>[] { curVelos, curPosition };
    }

    public static List<Vector3>[] SIEulerMethod(List<Vector3> ps, List<Vector3> vs, FlockingParameter info, Vector3 goal)
    {
        List<Vector3> curVelos = new List<Vector3>();
        List<Vector3> curPosition = new List<Vector3>();

        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps, vs, info, goal);
            Vector3 vector = vs[i] + a * info.t;
            curVelos.Add(vector);
            curPosition.Add(ps[i] + vector * info.t);
        }

        return new List<Vector3>[] { curVelos, curPosition };
    }

    public static List<Vector3>[] RK4Method(List<Vector3> ps, List<Vector3> vs, FlockingParameter info, Vector3 goal)
    {
        float t = info.t;

        List<Vector3> vs2 = new List<Vector3>();
        List<Vector3> ps2 = new List<Vector3>();
        List<Vector3> k1_a = new List<Vector3>();
        List<Vector3> k1_v = new List<Vector3>();
        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps, vs, info, goal);
            k1_a.Add(a);
            k1_v.Add(vs[i]);
            vs2.Add(vs[i] + 0.5f * t * k1_a[i]);
            ps2.Add(ps[i] + 0.5f * t * k1_v[i]);
        }

        List<Vector3> vs3 = new List<Vector3>();
        List<Vector3> ps3 = new List<Vector3>();
        List<Vector3> k2_a = new List<Vector3>();
        List<Vector3> k2_v = new List<Vector3>();
        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps2, vs2, info, goal);
            k2_a.Add(a);
            k2_v.Add(vs2[i]);
            vs3.Add(vs[i] + 0.5f * t * k2_a[i]);
            ps3.Add(ps[i] + 0.5f * t * k2_v[i]);
        }

        List<Vector3> vs4 = new List<Vector3>();
        List<Vector3> ps4 = new List<Vector3>();
        List<Vector3> k3_a = new List<Vector3>();
        List<Vector3> k3_v = new List<Vector3>();
        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps3, vs3, info, goal);
            k3_a.Add(a);
            k3_v.Add(vs3[i]);
            vs4.Add(vs[i] + t * k3_a[i]);
            ps4.Add(ps[i] + t * k3_v[i]);
        }

        List<Vector3> k4_a = new List<Vector3>();
        List<Vector3> k4_v = new List<Vector3>();
        for (int i = 0; i < info.n; i++)
        {
            Vector3 a = ForceFunc(i, ps4, vs4, info, goal);
            k4_a.Add(a);
            k4_v.Add(vs4[i]);
        }

        List<Vector3> curVelos = new List<Vector3>();
        List<Vector3> curPosition = new List<Vector3>();
        //Debug.Log()
        for (int i = 0; i < info.n; i++)
        {
            curVelos.Add(vs[i] + t / 6f * (k1_a[i] + 2f * k2_a[i] + 2f * k3_a[i] + k4_a[i]));
            curPosition.Add(ps[i] + t / 6f * (k1_v[i] + 2f * k2_v[i] + 2f * k3_v[i] + k4_v[i]));
        }
        return new List<Vector3>[] { curVelos, curPosition };
    }
}
