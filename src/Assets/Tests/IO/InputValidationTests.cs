using FlockingEntity;
using MyExceptions;
using NUnit.Framework;
using System.Collections.Generic;

public class InputValidationTests
{
    [Test]
    public void KVerify_Valid_Should_Return_Value()
    {
        float result = InputValidation.KVerify("1.5", InputValidation.InputType.KA);
        Assert.AreEqual(1.5f, result);
    }

    [Test]
    public void KVerify_Empty_Should_Throw_MissingK()
    {
        Assert.Throws<MissingK>(() =>
        {
            InputValidation.KVerify("", InputValidation.InputType.KA);
        });
    }

    [Test]
    public void KVerify_OutOfRange_Should_Throw_ErrorK()
    {
        Assert.Throws<ErrorK>(() =>
        {
            InputValidation.KVerify("10", InputValidation.InputType.KA);
        });
    }

    [Test]
    public void NVerify_Valid()
    {
        int result = InputValidation.NVerify("10");
        Assert.AreEqual(10, result);
    }

    [Test]
    public void NVerify_Invalid_Should_Throw_ErrorN()
    {
        Assert.Throws<ErrorN>(() =>
        {
            InputValidation.NVerify("10000");
        });
    }

    [Test]
    public void TVerify_Valid()
    {
        float result = InputValidation.TVerify("0.01");
        Assert.AreEqual(0.01f, result);
    }

    [Test]
    public void TVerify_Invalid_Should_Throw_ErrorT()
    {
        Assert.Throws<ErrorT>(() =>
        {
            InputValidation.TVerify("1");
        });
    }

    [Test]
    public void ObsVerify_Valid_Input()
    {
        string input = "(0,0),10;(100,100),20";

        List<Obstacle> result = InputValidation.ObsVerify(input);

        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public void ObsVerify_MissingRadius_Should_Throw()
    {
        string input = "0,0";

        Assert.Throws<MissingP>(() =>
        {
            InputValidation.ObsVerify(input);
        });
    }

    [Test]
    public void ObsVerify_InvalidPosition_Should_Throw()
    {
        string input = "(10000,0),10";

        Assert.Throws<ErrorP>(() =>
        {
            InputValidation.ObsVerify(input);
        });
    }


    [Test]
    public void MethodVerify_Valid()
    {
        Assert.DoesNotThrow(() =>
        {
            InputValidation.MethodVerify(InputValidation.EEULER);
        });
    }

    [Test]
    public void MethodVerify_Empty_Should_Throw()
    {
        Assert.Throws<MissingMethod>(() =>
        {
            InputValidation.MethodVerify("");
        });
    }

    [Test]
    public void MethodVerify_Invalid_Should_Throw()
    {
        Assert.Throws<ErrorMethod>(() =>
        {
            InputValidation.MethodVerify("WRONG");
        });
    }


    [Test]
    public void PathVerify_Valid()
    {
        Assert.DoesNotThrow(() =>
        {
            InputValidation.PathVerify("C:/test.json");
        });
    }

    [Test]
    public void PathVerify_Empty_Should_Throw()
    {
        Assert.Throws<MissingPath>(() =>
        {
            InputValidation.PathVerify("");
        });
    }
}