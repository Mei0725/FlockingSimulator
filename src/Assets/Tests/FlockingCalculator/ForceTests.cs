using FlockingEntity;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ForceTests
{
    [Test]
    public void CalAlignForce_Should_Point_To_Other_Velocity()
    {
        var vs = new List<Vector3>
        {
            new Vector3(1, 0, 0), // i = 0
            new Vector3(3, 0, 0)
        };

        Vector3 force = Force.calAlignForce(0, vs);

        // avg = 3 → 3 - 1 = 2
        Assert.Greater(force.x, 0);
    }

    [Test]
    public void CalAlignForce_Single_Should_Be_Zero()
    {
        var vs = new List<Vector3>
        {
            new Vector3(1, 0, 0)
        };

        Vector3 force = Force.calAlignForce(0, vs);

        Assert.AreEqual(Vector3.zero, force);
    }

    [Test]
    public void CalCohesionForce_Should_Point_To_Center()
    {
        var ps = new List<Vector3>
        {
            new Vector3(0, 0, 0), // i
            new Vector3(10, 0, 0)
        };

        Vector3 force = Force.calCohesionForce(0, ps);

        Assert.Greater(force.x, 0);
    }

    [Test]
    public void CalCohesionForce_Single_Should_Be_Zero()
    {
        var ps = new List<Vector3>
        {
            new Vector3(1, 1, 0)
        };

        Assert.AreEqual(Vector3.zero, Force.calCohesionForce(0, ps));
    }

    [Test]
    public void CalSeparationForce_Should_Repel()
    {
        var ps = new List<Vector3>
        {
            new Vector3(0, 0, 0), // i
            new Vector3(1, 0, 0)
        };

        Vector3 force = Force.calSeparationForce(0, ps);

        Assert.Less(force.x, 0); // repel left
    }

    [Test]
    public void CalSeparationForce_Single_Should_Be_Zero()
    {
        var ps = new List<Vector3>
        {
            new Vector3(0, 0, 0)
        };

        Assert.AreEqual(Vector3.zero, Force.calSeparationForce(0, ps));
    }

    [Test]
    public void CalObsAvoidForce_Should_Repel_From_Obstacle()
    {
        var obs = new List<Obstacle>
        {
            new Obstacle { p = new V3(0,0,0), r = 1f }
        };

        Vector3 p = new Vector3(1.5f, 0, 0);

        Vector3 force = Force.calObsAvoidForce(p, obs);

        Assert.Greater(force.x, 0);
    }

    [Test]
    public void CalObsAvoidForce_NoObstacle_Should_Be_Zero()
    {
        var obs = new List<Obstacle>();

        Vector3 force = Force.calObsAvoidForce(Vector3.zero, obs);

        Assert.AreEqual(Vector3.zero, force);
    }

    [Test]
    public void CalGoalSeekingForce_Should_Point_To_Goal()
    {
        Vector3 p = Vector3.zero;
        Vector3 goal = new Vector3(10, 0, 0);
        Vector3 v = Vector3.zero;

        Vector3 force = Force.calGoalSeekingForce(p, goal, v);

        Assert.Greater(force.x, 0);
    }

    [Test]
    public void CalGoalSeekingForce_WithVelocity_Should_Adjust()
    {
        Vector3 p = Vector3.zero;
        Vector3 goal = new Vector3(10, 0, 0);
        Vector3 v = new Vector3(5, 0, 0);

        Vector3 force = Force.calGoalSeekingForce(p, goal, v);

        Assert.Less(force.x, 20f); // vMax = 20
    }

    [Test]
    public void CalForce_Should_Return_Combined_Force()
    {
        var ps = new List<Vector3>
        {
            Vector3.zero,
            new Vector3(1,0,0)
        };

        var vs = new List<Vector3>
        {
            Vector3.zero,
            new Vector3(1,0,0)
        };

        var param = new FlockingParameter
        {
            ka = 1,
            ks = 1,
            kc = 1,
            ko = 1,
            kg = 1,
            obs = new List<Obstacle>()
        };

        Vector3 goal = new Vector3(10, 0, 0);

        Vector3 result = Force.CalForce(0, ps, vs, param, goal);

        Assert.AreNotEqual(Vector3.zero, result);
    }
}