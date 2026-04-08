using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using FlockingEntity;
using XCharts.Runtime;

public class ChartDrawerTests
{
    private GameObject root;
    private ChartDrawer drawer;

    [SetUp]
    public void Setup()
    {
        root = new GameObject("ChartDrawer");

        drawer = root.AddComponent<ChartDrawer>();

        drawer.speedChartObject = CreateChart("speed");
        drawer.alignChartObject = CreateChart("align");
        drawer.coheChartObject = CreateChart("cohe");
        drawer.speedDiffChartObject = CreateChart("speedDiff");
        drawer.alignDiffChartObject = CreateChart("alignDiff");
        drawer.coheDiffChartObject = CreateChart("coheDiff");

        drawer.Init();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);
    }

    [Test]
    public void Start_Should_Initialize_Charts()
    {
        Assert.NotNull(drawer);
    }


    [Test]
    public void DrawChart_Should_Not_Throw()
    {
        var data = CreateMockData(2);

        Assert.DoesNotThrow(() =>
        {
            drawer.DrawChart(data);
        });
    }


    [Test]
    public void DrawChart_Should_Add_Series()
    {
        var data = CreateMockData(2);

        drawer.DrawChart(data);

        var chart = drawer.speedChartObject.GetComponent<LineChart>();

        Assert.IsTrue(chart.series.Count > 0);
    }

    [Test]
    public void DrawChart_MultipleInputs_Should_Work()
    {
        var data = CreateMockData(3);

        drawer.DrawChart(data);

        var chart = drawer.speedDiffChartObject.GetComponent<LineChart>();

        Assert.IsTrue(chart.series.Count > 0);
    }

    private GameObject CreateChart(string name)
    {
        var obj = new GameObject(name);
        obj.AddComponent<LineChart>();
        return obj;
    }

    private List<FlockingStatus> CreateMockData(int count)
    {
        var list = new List<FlockingStatus>();

        for (int i = 0; i < count; i++)
        {
            list.Add(new FlockingStatus
            {
                t = 1.0f,
                meanSpeed = new List<float> { 1, 2, 3 },
                alignMetric = new List<float> { 0.1f, 0.2f, 0.3f },
                cohesion = new List<float> { 5, 6, 7 }
            });
        }

        return list;
    }
}