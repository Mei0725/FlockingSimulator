using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using FlockingEntity;

public class MockCameraManager : MonoBehaviour
{
    public bool getAgentsCalled = false;
    public bool updateCenterCalled = false;
    public bool destroyAgentsCalled = false;

    public void getAgents(string tag)
    {
        getAgentsCalled = true;
    }

    public void UpdateCameraCenter()
    {
        updateCenterCalled = true;
    }

    public void DestroyAgents()
    {
        destroyAgentsCalled = true;
    }
}

public class AnimationSimulatorTests
{
    private GameObject simulatorObj;
    private AnimationSimulator simulator;

    private GameObject cam;
    private GameObject animeUI;
    private GameObject labels;

    [SetUp]
    public void Setup()
    {
        simulatorObj = new GameObject();
        simulator = simulatorObj.AddComponent<AnimationSimulator>();

        cam = new GameObject();
        cam.AddComponent<MockCameraManager>();

        animeUI = new GameObject();
        labels = new GameObject();

        simulator.cam = cam;
        simulator.animeUI = animeUI;
        simulator.labels = labels;
        simulator.obsMaterial = new Material(Shader.Find("Standard"));

        simulator.init();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(simulatorObj);
        Object.DestroyImmediate(cam);
        Object.DestroyImmediate(animeUI);
        Object.DestroyImmediate(labels);
    }


    [UnityTest]
    public IEnumerator PlaySimulation_SingleAnimation_StartsCorrectly()
    {
        var anime = CreateValidAnimation();

        simulator.PlaySimulation(anime);

        Assert.IsFalse(animeUI.activeSelf);

        yield return null;

        var agents = GameObject.FindGameObjectsWithTag("Agent");
        Assert.IsTrue(agents.Length > 0);
    }

    [Test]
    public void PlaySimulation_WhenAlreadyPlaying_ShouldNotStart()
    {
        var anime = CreateValidAnimation();

        simulator.PlaySimulation(anime);
        simulator.PlaySimulation(anime);

        Assert.Pass();
    }

    [Test]
    public void PlaySimulation_MultipleAnimations_EnableLabels()
    {
        var list = new List<AnimationStatus>()
        {
            CreateValidAnimation(),
            CreateValidAnimation()
        };

        simulator.PlaySimulation(list);

        Assert.IsTrue(labels.activeSelf);
    }

    [UnityTest]
    public IEnumerator Simulation_EmptyAgents_ShouldExitEarly()
    {
        var anime = new AnimationStatus
        {
            agents = new List<Agents>(),
            obs = new List<Obstacle>(),
            t = 0.01f
        };

        simulator.PlaySimulation(anime);

        yield return null;

        Assert.IsTrue(animeUI.activeSelf);
        Assert.IsFalse(labels.activeSelf);
    }

    [Test]
    public void DrawObstacles_ShouldCreateGameObjects()
    {
        var anime = CreateValidAnimation();

        simulator.PlaySimulation(anime);

        var spheres = GameObject.FindObjectsOfType<SphereCollider>();
        Assert.IsTrue(spheres.Length > 0);
    }

    private AnimationStatus CreateValidAnimation()
    {
        return new AnimationStatus
        {
            t = 0.01f,
            obs = new List<Obstacle>()
            {
                new Obstacle
                {
                    p = new V3(0, 0, 0),
                    r = 1
                }
            },
            agents = new List<Agents>()
            {
                new Agents
                {
                    positions = new List<V3>()
                    {
                        new V3(0, 0, 0),
                        new V3(1, 1, 0),
                    }
                }
            }
        };
    }
}