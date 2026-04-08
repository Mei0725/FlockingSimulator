using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerTests
{
    private GameObject root;
    private UIController controller;

    private Button CreateButton()
    {
        var go = new GameObject("Button");
        var button = go.AddComponent<Button>();

        var cb = button.colors;
        cb.normalColor = Color.white;
        button.colors = cb;

        return button;
    }

    private GameObject CreateObject()
    {
        return new GameObject("Obj");
    }

    [SetUp]
    public void Setup()
    {
        root = new GameObject("UIControllerRoot");
        controller = root.AddComponent<UIController>();

        controller.buttons = new Button[3];
        controller.objects = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            controller.buttons[i] = CreateButton();
            controller.objects[i] = CreateObject();
        }
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);
    }

    [Test]
    public void Start_Should_Activate_Index0_Object()
    {
        controller.Init();

        Assert.IsTrue(controller.objects[0].activeSelf);
        Assert.IsFalse(controller.objects[1].activeSelf);
        Assert.IsFalse(controller.objects[2].activeSelf);
    }

    [Test]
    public void OnButtonClick_Should_Activate_Correct_Object()
    {
        controller.OnButtonClick(1);

        Assert.IsFalse(controller.objects[0].activeSelf);
        Assert.IsTrue(controller.objects[1].activeSelf);
        Assert.IsFalse(controller.objects[2].activeSelf);
    }

    [Test]
    public void OnButtonClick_Should_Update_CurrentIndex()
    {
        controller.OnButtonClick(2);

        Assert.IsTrue(controller.objects[2].activeSelf);
    }

    [Test]
    public void UpdateButtonColors_Should_Select_Correct_Button()
    {
        controller.OnButtonClick(1);

        for (int i = 0; i < controller.buttons.Length; i++)
        {
            var color = controller.buttons[i].colors.normalColor;

            if (i == 1)
            {
                Assert.AreEqual(controller.selectedColor, color);
            }
            else
            {
                Assert.AreEqual(controller.normalColor, color);
            }
        }
    }
}