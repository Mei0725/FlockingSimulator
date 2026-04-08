using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FlockingMetricsTests
{
    [Test]
    public void CalMeanSpeed_Should_Return_Correct_Average()
    {
        var velos = new List<Vector3>
        {
            new Vector3(3, 0, 0), // magnitude = 3
            new Vector3(4, 0, 0)  // magnitude = 4
        };

        float result = FlockingMetrics.CalMeanSpeed(velos);

        Assert.AreEqual(3.5f, result, 1e-5f);
    }


    [Test]
    public void CalAlignMetric_SameDirection_Should_Be_1()
    {
        var velos = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(2, 0, 0)
        };

        float result = FlockingMetrics.CalAlignMetric(velos);

        Assert.AreEqual(1f, result, 1e-5f);
    }

    [Test]
    public void CalAlignMetric_OppositeDirection_Should_Be_0()
    {
        var velos = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0)
        };

        float result = FlockingMetrics.CalAlignMetric(velos);

        Assert.AreEqual(0f, result, 1e-5f);
    }


    [Test]
    public void CalCenter_Should_Return_Correct_Center()
    {
        var positions = new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 2, 0)
        };

        Vector3 center = FlockingMetrics.CalCenter(positions);

        Assert.AreEqual(new Vector3(1, 1, 0), center);
    }


    [Test]
    public void CalCohesion_Should_Calculate_Average_Distance()
    {
        var positions = new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 0, 0)
        };

        float result = FlockingMetrics.CalCohesion(positions);

        // center = (1,0,0)
        // distances = 1 + 1 → avg = 1
        Assert.AreEqual(1f, result, 1e-5f);
    }


    [Test]
    public void CalCohesion_WithCenter_Should_Match()
    {
        var positions = new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 0, 0)
        };

        Vector3 center = new Vector3(1, 0, 0);

        float result = FlockingMetrics.CalCohesion(center, positions);

        Assert.AreEqual(1f, result, 1e-5f);
    }


    [Test]
    public void CalMeanSpeed_SingleElement()
    {
        var velos = new List<Vector3>
        {
            new Vector3(5, 0, 0)
        };

        float result = FlockingMetrics.CalMeanSpeed(velos);

        Assert.AreEqual(5f, result);
    }
}