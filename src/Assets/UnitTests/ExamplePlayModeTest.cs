using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExamplePlayModeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExamplePlayModeTest1SimplePasses()
    {
        int a = 2;
        int b = 3;

        Assert.AreEqual(5, a + b);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ExamplePlayModeTest1WithEnumeratorPasses()
    {
        GameObject obj = new GameObject("TestObject");

        yield return null;

        Assert.NotNull(obj);

        Object.Destroy(obj);
    }
}
