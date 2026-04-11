using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChartsControllerTests
{
    private GameObject root;
    private ChartsController controller;

    [SetUp]
    public void Setup()
    {
        root = new GameObject("ChartsController");
        controller = root.AddComponent<ChartsController>();

        controller.controller = CreateGO("controller");
        controller.charts = CreateGO("charts");

        controller.chartObjects = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            controller.chartObjects.Add(CreateGO("chart_" + i));
        }
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);

        foreach (var obj in controller.chartObjects)
        {
            if (obj != null)
                Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void Start_Should_Set_UI_State()
    {
        controller.Init();

        Assert.IsTrue(controller.controller.activeSelf);
        Assert.IsFalse(controller.charts.activeSelf);
    }

    [Test]
    public void OnDrawButton_Should_Switch_UI()
    {
        controller.OnDrawButton();

        Assert.IsFalse(controller.controller.activeSelf);
        Assert.IsTrue(controller.charts.activeSelf);
    }

    [Test]
    public void OnBackButton_Should_Switch_Back()
    {
        controller.OnDrawButton();
        controller.OnBackButton();

        Assert.IsTrue(controller.controller.activeSelf);
        Assert.IsFalse(controller.charts.activeSelf);
    }

    [Test]
    public void OnSpeedClick_Should_Set_Index_0()
    {
        controller.OnSpeedClick();

        AssertChartActive(0);
    }

    [Test]
    public void OnAlignClick_Should_Set_Index_1_Mode()
    {
        controller.OnAlignClick();

        AssertChartActive(2);
    }

    [Test]
    public void OnCoheClick_Should_Set_Index_2_Mode()
    {
        controller.OnCoheClick();

        AssertChartActive(4);
    }

    [Test]
    public void OnDiffClick_Should_Switch_To_Diff_Mode()
    {
        controller.OnSpeedClick(); // chartIndex = 0
        controller.OnDiffClick();   // isDiff = 1

        AssertChartActive(1);
    }

    [Test]
    public void OnOverviewClick_Should_Switch_Back()
    {
        controller.OnSpeedClick();
        controller.OnDiffClick();
        controller.OnOverViewClick();

        AssertChartActive(0);
    }

    private GameObject CreateGO(string name)
    {
        var obj = new GameObject(name);
        obj.SetActive(true);
        return obj;
    }

    private void AssertChartActive(int index)
    {
        for (int i = 0; i < controller.chartObjects.Count; i++)
        {
            if (i == index)
                Assert.IsTrue(controller.chartObjects[i].activeSelf, $"index {i} should be active");
            else
                Assert.IsFalse(controller.chartObjects[i].activeSelf, $"index {i} should be inactive");
        }
    }
}