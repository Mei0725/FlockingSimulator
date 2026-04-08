using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraManagerTests
{
    private GameObject cameraObj;
    private CameraManager manager;

    [SetUp]
    public void Setup()
    {
        cameraObj = new GameObject("Camera");
        cameraObj.AddComponent<Camera>();
        manager = cameraObj.AddComponent<CameraManager>();

        manager.offset = new Vector3(0, 0, -10);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(cameraObj);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void GetAgents_Should_Filter_By_Prefix()
    {
        CreateAgent("AgentA_1");
        CreateAgent("AgentA_2");
        CreateAgent("AgentB_1");

        manager.getAgents("AgentA");

        var field = typeof(CameraManager)
            .GetField("agents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var agents = (GameObject[])field.GetValue(manager);

        Assert.AreEqual(2, agents.Length);
    }

    [Test]
    public void DestroyAgents_Should_Clear_Array()
    {
        CreateAgent("AgentA_1");
        manager.getAgents("AgentA");

        manager.DestroyAgents();

        var field = typeof(CameraManager)
            .GetField("agents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var agents = field.GetValue(manager);

        Assert.IsNull(agents);
    }

    [UnityTest]
    public IEnumerator UpdateCameraCenter_Should_Move_Camera()
    {
        var a1 = CreateAgent("AgentA_1", new Vector3(0, 0, 0));
        var a2 = CreateAgent("AgentA_2", new Vector3(10, 0, 0));

        manager.getAgents("AgentA");

        yield return null;

        Vector3 before = cameraObj.transform.position;

        manager.UpdateCameraCenter();

        yield return null;

        Vector3 after = cameraObj.transform.position;

        Assert.AreNotEqual(before, after);
    }

    [UnityTest]
    public IEnumerator UpdateCameraCenter_Should_Change_Zoom()
    {
        var cam = cameraObj.GetComponent<Camera>();

        CreateAgent("AgentA_1", new Vector3(0, 0, 0));
        CreateAgent("AgentA_2", new Vector3(20, 0, 0));

        manager.getAgents("AgentA");

        yield return null;

        float before = cam.orthographicSize;

        manager.UpdateCameraCenter();

        yield return null;

        float after = cam.orthographicSize;

        Assert.AreNotEqual(before, after);
    }

    [Test]
    public void UpdateCameraCenter_NoAgents_Should_Not_Crash()
    {
        manager.DestroyAgents();

        Assert.DoesNotThrow(() => manager.UpdateCameraCenter());
    }

    private GameObject CreateAgent(string name, Vector3 pos = default)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = name;
        obj.tag = "Agent";
        obj.transform.position = pos;
        return obj;
    }
}
