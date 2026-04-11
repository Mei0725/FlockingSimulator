using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using FlockingEntity;

public static class FakeForce
{
    public static Vector3 fixedForce = new Vector3(1, 0, 0);

    public static Vector3 CalForce(int i, List<Vector3> ps, List<Vector3> vs, FlockingParameter info, Vector3 goal)
    {
        return fixedForce;
    }
}

public class ODESolverTests
{
    [SetUp]
    public void Setup()
    {
        ODESolver.ForceFunc = FakeForce.CalForce;
    }

    private FlockingParameter CreateParam(int n, float t)
    {
        return new FlockingParameter
        {
            n = n,
            t = t
        };
    }

    private List<Vector3> CreateList(int n)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < n; i++)
            list.Add(Vector3.one * i);
        return list;
    }

    [Test]
    public void EEulerMethod_Should_Return_Correct_Size()
    {
        var ps = CreateList(3);
        var vs = CreateList(3);
        var info = CreateParam(3, 0.1f);

        var result = ODESolver.EEulerMethod(ps, vs, info, Vector3.zero);

        Assert.AreEqual(2, result.Length);
        Assert.AreEqual(3, result[0].Count);
        Assert.AreEqual(3, result[1].Count);
    }

    [Test]
    public void SIEulerMethod_Should_Update_Position_And_Velocity()
    {
        var ps = CreateList(2);
        var vs = CreateList(2);
        var info = CreateParam(2, 0.1f);

        var result = ODESolver.SIEulerMethod(ps, vs, info, Vector3.zero);

        Assert.AreEqual(2, result.Length);
        Assert.AreEqual(2, result[0].Count);
        Assert.AreEqual(2, result[1].Count);

        // 基本数值 sanity check
        Assert.AreNotEqual(ps[0], result[1][0]);
    }

    [Test]
    public void RK4Method_Should_Return_Valid_Output()
    {
        var ps = CreateList(2);
        var vs = CreateList(2);
        var info = CreateParam(2, 0.1f);

        var result = ODESolver.RK4Method(ps, vs, info, Vector3.zero);

        Assert.AreEqual(2, result.Length);
        Assert.AreEqual(2, result[0].Count);
        Assert.AreEqual(2, result[1].Count);

        Assert.IsFalse(float.IsNaN(result[0][0].x));
        Assert.IsFalse(float.IsNaN(result[1][0].x));
    }

    [Test]
    public void EEuler_Should_Not_Throw()
    {
        var ps = CreateList(5);
        var vs = CreateList(5);
        var info = CreateParam(5, 0.02f);

        Assert.DoesNotThrow(() =>
        {
            ODESolver.EEulerMethod(ps, vs, info, Vector3.zero);
        });
    }
}