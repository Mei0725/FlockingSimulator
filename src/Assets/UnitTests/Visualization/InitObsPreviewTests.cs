using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class InitObsPreviewTests
{
    private GameObject root;
    private InitObsPreview preview;


    [SetUp]
    public void Setup()
    {
        root = new GameObject("Preview");

        preview = root.AddComponent<InitObsPreview>();

        // env
        preview.envBackground = new GameObject("Env").transform;
        preview.obsPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var inputGO = new GameObject("Input");
        preview.obsInput = inputGO.AddComponent<TMP_InputField>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);
    }


    [UnityTest]
    public IEnumerator OnPreviewClick_Should_Create_Obstacles()
    {
        preview.obsInput.text = "(0,0),10;(100,100),20";

        preview.OnPreviewClick();

        yield return null;

        Assert.Greater(preview.envBackground.childCount, 0);
    }


    [UnityTest]
    public IEnumerator OnPreviewClick_Should_Clear_Old_Objects()
    {
        preview.obsInput.text = "0,0,10";

        preview.OnPreviewClick();
        yield return null;

        int firstCount = preview.envBackground.childCount;

        preview.obsInput.text = "100,100,20";
        preview.OnPreviewClick();

        yield return null;

        int secondCount = preview.envBackground.childCount;

        Assert.AreEqual(1, secondCount);
    }

    [UnityTest]
    public IEnumerator OnPreviewClick_Should_Set_Position()
    {
        preview.obsInput.text = "(250,250),10";

        preview.OnPreviewClick();

        yield return null;

        var obj = preview.envBackground.GetChild(0);

        Assert.AreEqual(Vector3.zero.x, obj.localPosition.x, 1e-3f);
        Assert.AreEqual(Vector3.zero.y, obj.localPosition.y, 1e-3f);
    }


    [UnityTest]
    public IEnumerator OnPreviewClick_Should_Set_Scale()
    {
        preview.obsInput.text = "(0,0),50";

        preview.OnPreviewClick();

        yield return null;

        var obj = preview.envBackground.GetChild(0);

        Assert.Greater(obj.localScale.x, 0);
    }


    [UnityTest]
    public IEnumerator OnPreviewClick_EmptyInput_Should_Not_Crash()
    {
        preview.obsInput.text = "";

        Assert.DoesNotThrow(() =>
        {
            preview.OnPreviewClick();
        });

        yield return null;

        Assert.AreEqual(0, preview.envBackground.childCount);
    }
}