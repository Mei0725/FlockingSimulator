using MyExceptions;
using NUnit.Framework;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class FilesIOTests
{
    private string testPath;

    [SetUp]
    public void Setup()
    {
        testPath = Path.Combine(Application.persistentDataPath, "test_file.json");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(testPath))
        {
            File.Delete(testPath);
        }
    }

    [Test]
    public void Write_Should_Create_File()
    {
        TestData data = new TestData { value = 42 };

        FilesIO.Write(data, testPath);

        Assert.IsTrue(File.Exists(testPath));
    }

    [Test]
    public void Write_Should_Contain_Correct_JSON()
    {
        TestData data = new TestData { value = 123 };

        FilesIO.Write(data, testPath);

        string json = File.ReadAllText(testPath);

        Assert.IsTrue(json.Contains("123"));
    }

    [Test]
    public void Read_Should_Return_Data()
    {
        TestData data = new TestData { value = 99 };
        FilesIO.Write(data, testPath);

        TestData result = FilesIO.Read<TestData>(testPath);

        Assert.AreEqual(99, result.value);
    }

    [Test]
    public void Read_Should_Throw_When_File_Not_Found()
    {
        if (File.Exists(testPath))
            File.Delete(testPath);

        Assert.Throws<NoFileFound>(() =>
        {
            FilesIO.Read<TestData>(testPath);
        });
    }

    [System.Serializable]
    public class TestData
    {
        public int value;
    }
}