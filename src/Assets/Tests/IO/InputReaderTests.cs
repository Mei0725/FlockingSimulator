using FlockingEntity;
using MyExceptions;
using NUnit.Framework;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.Networking.UnityWebRequest;

public class InputReaderTests
{
    private GameObject go;
    private InputReader reader;

    private TMP_InputField CreateInput(string text = "")
    {
        var obj = new GameObject();
        var input = obj.AddComponent<TMP_InputField>();
        input.text = text;
        return input;
    }

    private TMP_Dropdown CreateDropdown(int value = 0)
    {
        var obj = new GameObject();
        var dropdown = obj.AddComponent<TMP_Dropdown>();

        dropdown.options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>
    {
        new TMP_Dropdown.OptionData("EEULER"),
        new TMP_Dropdown.OptionData("RK4"),
        new TMP_Dropdown.OptionData("ISEULER")
    };

        dropdown.value = value;

        return dropdown;
    }

    [SetUp]
    public void Setup()
    {
        go = new GameObject("Reader");
        reader = go.AddComponent<InputReader>();

        reader.kaInput = CreateInput("1");
        reader.kcInput = CreateInput("1");
        reader.ksInput = CreateInput("1");
        reader.koInput = CreateInput("1");
        reader.kgInput = CreateInput("1");
        reader.tInput = CreateInput("0.01");
        reader.nInput = CreateInput("100");
        reader.obsInput = CreateInput("(100,100),50");

        reader.methodInput = CreateDropdown(0);

        reader.initPathInput = CreateInput();
        reader.animaPathInput = CreateInput();
        reader.flockPathInput = CreateInput();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void GetUserInput_Should_Return_Valid_Object()
    {
        var result = reader.GetUserInput();

        Assert.AreEqual(1f, result.ka);
        Assert.AreEqual(100, result.n);
        Assert.AreEqual(1, result.obs.Count);
    }


    [Test]
    public void GetSimulateParam_InvalidPath_Should_Throw()
    {
        reader.initPathInput.text = "";

        Assert.Throws<MissingPath>(() =>
        {
            reader.GetSimulateParam();
        });
    }

    [Test]
    public void GetMethod_Should_Map_Correctly()
    {
        reader.methodInput.value = 1;

        string method = reader.GetMethod();

        Assert.AreEqual(InputValidation.RK4, method);
    }


    [UnityTest]
    public IEnumerator GetAnimaStatus_Should_Read_Multiple_Files()
    {
        string path = Application.dataPath + "/test_anim.json";

        var dummy = new AnimationStatus();
        File.WriteAllText(path, JsonUtility.ToJson(dummy));

        reader.animaPathInput.text = path + ";" + path;

        var result = reader.GetAnimaStatus();

        yield return null;

        Assert.AreEqual(2, result.Count);

        File.Delete(path);
    }


    [UnityTest]
    public IEnumerator GetFlockStatus_Should_Read_Files()
    {
        string path = Application.dataPath + "/test_flock.json";

        var dummy = new FlockingStatus();
        File.WriteAllText(path, JsonUtility.ToJson(dummy));

        reader.flockPathInput.text = path;

        var result = reader.GetFlockStatus();

        yield return null;

        Assert.AreEqual(1, result.Count);

        File.Delete(path);
    }


    [Test]
    public void GetAnimaStatus_Empty_Should_Return_Empty_List()
    {
        reader.animaPathInput.text = ";;";

        var result = reader.GetAnimaStatus();

        Assert.AreEqual(0, result.Count);
    }

    private void SetInputValue(string ka, string kc, string ks, string ko, string kg, string t, string n, string obs, int method)
    {

        reader.kaInput = CreateInput(ka);
        reader.kcInput = CreateInput(kc);
        reader.ksInput = CreateInput(ks);
        reader.koInput = CreateInput(ko);
        reader.kgInput = CreateInput(kg);
        reader.tInput = CreateInput(t);
        reader.nInput = CreateInput(n);
        reader.obsInput = CreateInput(obs);
        reader.methodInput = CreateDropdown(method);
    }

    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "-10", "", 2, typeof(ErrorN))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "-0.01", "10", "", 2, typeof(ErrorT))]
    [TestCase("3.5", "0.5", "0.5", "0.5", "0.5", "0.01", "10", "", 2, typeof(ErrorK))]
    [TestCase("0.5", "3.5", "0.5", "0.5", "0.5", "0.01", "10", "", 2, typeof(ErrorK))]
    [TestCase("0.5", "0.5", "3.5", "0.5", "0.5", "0.01", "10", "", 2, typeof(ErrorK))]
    [TestCase("0.5", "0.5", "0.5", "3.5", "0.5", "0.01", "10", "", 2, typeof(ErrorK))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "3.5", "0.01", "10", "", 2, typeof(ErrorK))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "", "", 2, typeof(MissingN))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "", "10", "", 2, typeof(MissingT))]
    [TestCase("", "0.5", "0.5", "0.5", "0.5", "0.01", "10", "", 2, typeof(MissingK))]
    [TestCase("0.5", "", "0.5", "0.5", "0.5", "0.01", "10", "", 2, typeof(MissingK))]
    [TestCase("0.5", "0.5", "", "0.5", "0.5", "0.01", "10", "", 2, typeof(MissingK))]
    [TestCase("0.5", "0.5", "0.5", "", "0.5", "0.01", "10", "", 2, typeof(MissingK))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "", "0.01", "10", "", 2, typeof(MissingK))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "10", "(-900,100),50", 2, typeof(ErrorP))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "10", "(100,100),-50", 2, typeof(ErrorR))]
    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "10", ",50", 2, typeof(MissingP))]
    public void Functional_Test_Exception(string ka, string kc, string ks, string ko, string kg, string t, string n, string obs, int method, System.Type expectedException)
    {
        SetInputValue(ka, kc, ks, ko, kg, t, n, obs, method);

        Assert.Throws(expectedException, () =>
        {
            reader.GetUserInput();
        });
    }

    [TestCase("0.5", "0.5", "0.5", "0.5", "0.5", "0.01", "10", "", 2)]
    public void Functional_Test_Obs(string ka, string kc, string ks, string ko, string kg, string t, string n, string obs, int method)
    {
        SetInputValue(ka, kc, ks, ko, kg, t, n, obs, method);

        FlockingParameter result = reader.GetUserInput();

        Assert.AreEqual(0, result.obs.Count);
    }
}