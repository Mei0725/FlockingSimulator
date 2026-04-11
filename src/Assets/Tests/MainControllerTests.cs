using FlockingEntity;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.Networking.UnityWebRequest;


public class FakeInputReader : InputReader
{
    public bool throwError = false;

    public override FlockingParameter GetUserInput()
    {
        if (throwError) throw new System.Exception("input error");

        return new FlockingParameter
        {
            n = 2,
            t = 0.01f,
            ka = 1,
            kc = 1,
            ks = 1,
            ko = 1,
            kg = 1,
            obs = new List<Obstacle>()
        };
    }

    public override FlockingParameter GetSimulateParam()
    {
        if (throwError) throw new System.Exception("simulate error");

        return new FlockingParameter
        {
            n = 2,
            t = 0.01f,
            obs = new List<Obstacle>()
        };
    }

    public override string GetMethod()
    {
        if (throwError) throw new System.Exception("method error");
        return InputValidation.EEULER;
    }

    public override List<AnimationStatus> GetAnimaStatus()
    {
        if (throwError) throw new System.Exception("anima error");
        return new List<AnimationStatus> { new AnimationStatus() };
    }

    public override List<FlockingStatus> GetFlockStatus()
    {
        if (throwError) throw new System.Exception("flock error");
        return new List<FlockingStatus> { new FlockingStatus() };
    }
}
public class FakeDynamics : Dynamics
{
    public bool called = false;

    public override (AnimationStatus anime, FlockingStatus flock) FlockSimulation(FlockingParameter param, string method)
    {
        called = true;
        AnimationStatus anime = new AnimationStatus();
        FlockingStatus flock = new FlockingStatus();

        return (anime, flock);
    }
}

public class FakeAnimationSimulator : AnimationSimulator
{
    public bool called = false;

    protected void Start() { }

    public override void PlaySimulation(List<AnimationStatus> a)
    {
        called = true;
    }
}

public class FakeChartDrawer : ChartDrawer
{
    public bool called = false;
    protected void Start() { }

    public override void DrawChart(List<FlockingStatus> f)
    {
        called = true;
    }
}


public class MainControllerTests
{
    private GameObject go;
    private MainController controller;
    private FakeInputReader input;
    private FakeAnimationSimulator anime;
    private FakeChartDrawer chart;
    private FakeDynamics dynamics;

    private TMP_Text CreateText()
    {
        return new GameObject().AddComponent<TextMeshProUGUI>();
    }

    [SetUp]
    public void Setup()
    {
        go = new GameObject("Main");

        controller = go.AddComponent<MainController>();

        var ctrlObj = new GameObject("Controller");
        controller.controller = ctrlObj;

        input = ctrlObj.AddComponent<FakeInputReader>();
        anime = ctrlObj.AddComponent<FakeAnimationSimulator>();
        chart = ctrlObj.AddComponent<FakeChartDrawer>();
        dynamics = new FakeDynamics();

        controller.initResult = CreateText();
        controller.simuResult = CreateText();
        controller.animeResult = CreateText();
        controller.chartResult = CreateText();

        controller.AwakeInit();
        controller.InjectDependencies(input, anime, chart, dynamics);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }


    [Test]
    public void InitClicked_Success_Should_Set_Green_Text()
    {
        controller.InitClicked();

        Assert.AreEqual(new Color(0, 200, 0), controller.initResult.color);
        Assert.IsTrue(controller.initResult.text.Contains("saved"));
    }

    [Test]
    public void InitClicked_Error_Should_Set_Red_Text()
    {
        input.throwError = true;

        controller.InitClicked();

        Assert.AreEqual(Color.red, controller.initResult.color);
    }


    [Test]
    public void CalculateClicked_Success()
    {
        controller.CalculateClicked();

        Assert.AreEqual(new Color(0, 200, 0), controller.simuResult.color);
    }

    [Test]
    public void CalculateClicked_Error()
    {
        input.throwError = true;

        controller.CalculateClicked();

        Assert.AreEqual(Color.red, controller.simuResult.color);
    }


    [Test]
    public void PlayAnimaClicked_Should_Call_Simulator()
    {
        controller.PlayAnimaClicked();

        Assert.IsTrue(anime.called);
    }

    [Test]
    public void PlayAnimaClicked_Error()
    {
        input.throwError = true;

        controller.PlayAnimaClicked();

        Assert.AreEqual(Color.red, controller.animeResult.color);
    }


    [Test]
    public void DrawClicked_Should_Call_DrawChart()
    {
        controller.DrawClicked();

        Assert.IsTrue(chart.called);
    }

    [Test]
    public void DrawClicked_Error()
    {
        input.throwError = true;

        controller.DrawClicked();

        Assert.AreEqual(Color.red, controller.chartResult.color);
    }
}