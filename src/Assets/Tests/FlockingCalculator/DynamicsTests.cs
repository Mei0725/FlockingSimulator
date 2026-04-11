using FlockingEntity;
using MyExceptions;
using NUnit.Framework;
using System.Collections.Generic;

public class DynamicsTests
{
    private Dynamics dynamics;

    [SetUp]
    public void Setup()
    {
        dynamics = new Dynamics();
    }

    private FlockingParameter CreateSimpleParam()
    {
        return new FlockingParameter
        {
            t = 0.1f,
            starts = new List<V3>
            {
                new V3(0, 0, 0),
                new V3(1, 1, 0)
            },
            ka = 0.1f,
            kc = 0.1f,
            ks = 0.1f,
            kg = 0.1f,
            ko = 0.1f,
            n = 2,
            obs = new List<Obstacle>()
        };
    }

    [Test]
    public void FlockSimulation_Should_Return_Result()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.EEULER);

        Assert.IsNotNull(result.anime);
        Assert.IsNotNull(result.flock);
    }

    [Test]
    public void FlockSimulation_Should_Initialize_Animation()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.EEULER);

        Assert.IsTrue(result.anime.agents.Count > 0);
    }

    [Test]
    public void FlockSimulation_Should_Generate_Metrics()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.EEULER);

        Assert.IsTrue(result.flock.meanSpeed.Count > 0);
        Assert.IsTrue(result.flock.alignMetric.Count > 0);
        Assert.IsTrue(result.flock.cohesion.Count > 0);
    }

    [Test]
    public void FlockSimulation_Should_Run_EEuler()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.EEULER);

        Assert.IsNotNull(result.flock);
    }

    [Test]
    public void FlockSimulation_Should_Run_RK4()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.RK4);

        Assert.IsNotNull(result.flock);
    }

    [Test]
    public void FlockSimulation_Should_Run_ISEULER()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.SIEULER);

        Assert.IsNotNull(result.flock);
    }

    [Test]
    public void FlockSimulation_Should_Handle_Invalid_Method()
    {
        var param = CreateSimpleParam();

        Assert.That(() =>
        {
            dynamics.FlockSimulation(param, "INVALID_METHOD");
        }, Throws.TypeOf<ErrorMethod>());
    }

    [Test]
    public void FlockSimulation_Should_Append_Positions()
    {
        var param = CreateSimpleParam();

        var result = dynamics.FlockSimulation(param, InputValidation.EEULER);

        Assert.IsTrue(result.anime.agents.Count >= 1);
    }
}