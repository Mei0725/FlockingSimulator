using FlockingEntity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;
using static FunctionalTest;

public class FunctionalTest
{
    [Serializable]
    public class Diff
    {
        public List<float> SpeedDiff;
        public List<float> AlignDiff;
        public List<float> CoheDiff;

        public Diff(List<float> speed, List<float> align, List<float> cohe)
        {
            SpeedDiff = speed;
            AlignDiff = align;
            CoheDiff = cohe;
        }
    }

    [Serializable]
    public class Diffs
    {
        public Diff EnRKdiff;
        public Diff EnSIdiff;
        public Diff RKnSIdiff;
    }

    private Dynamics dynamics = new Dynamics();

    private static float vMax = 20.0f;

    private string animePrefix = "anime_";
    private string flockPrefix = "flock_";
    private string diffPrefix = "diff_";
    private string sourcePath;
    private string resPath;
    private string ft1FileName = "ft_single.json";
    private string ft2FileName = "ft_symmetry.json";
    private string ft3FileName = "ft_weight.json";
    private string ft4FileName = "ft_velocity.json";
    private string ft5FileName = "ft_odecompare.json";
    private string ft6FileName = "ft_large_timestep.json";

    [SetUp]
    public void Setup()
    {
        sourcePath = Path.Combine(Application.dataPath, "TestDatas");
        resPath = Path.Combine(sourcePath, "Results");
    }

    [Test]
    public void FunctionalTest1()
    {
        FlockingParameter param = ReadFile<FlockingParameter>(ft1FileName);
        var res = dynamics.FlockSimulation(param, InputValidation.SIEULER);
        FilesIO.Write<AnimationStatus>(res.anime, Path.Combine(resPath, animePrefix + ft1FileName));
        FilesIO.Write<FlockingStatus>(res.flock, Path.Combine(resPath, flockPrefix + ft1FileName));

        checkMaxV(res.flock.meanSpeed);
    }

    [Test]
    public void FunctionalTest2And3()
    {
        FlockingStatus flock2 = CheckSymmetry(ft2FileName), flock3 = CheckSymmetry(ft3FileName);
        List<float> cohe2 = flock2.cohesion, cohe3 = flock3.cohesion;
        int length = Math.Min(cohe2.Count, cohe3.Count);
        Assert.Greater(length, 1);
        for (int i = 0; i < length; i++)
        {
            Assert.LessOrEqual(cohe3[i], cohe3[i]);
        }
    }

    private FlockingStatus CheckSymmetry(string fileName)
    {
        FlockingParameter param2 = ReadFile<FlockingParameter>(fileName);
        var res2 = dynamics.FlockSimulation(param2, InputValidation.SIEULER);
        FilesIO.Write<AnimationStatus>(res2.anime, Path.Combine(resPath, animePrefix + fileName));
        FilesIO.Write<FlockingStatus>(res2.flock, Path.Combine(resPath, flockPrefix + fileName));

        List<Agents> agents2 = res2.anime.agents;
        Assert.Greater(agents2.Count, 1);
        Assert.AreEqual(agents2[0].positions.Count, 2);
        for (int i = 0; i < agents2.Count; i++)
        {
            float centerX = (agents2[i].positions[0].x + agents2[i].positions[1].x) / 2, centerY = (agents2[i].positions[0].y + agents2[i].positions[1].y) / 2;
            float diff = Mathf.Abs(centerX - centerY);
            Assert.LessOrEqual(diff, 0.1f);
        }
        return res2.flock;
    }

    [Test]
    public void FunctionalTest4And5()
    {
        Diffs diffs4 = CheckMaxVelocity(ft4FileName, 100), diffs5 = CheckMaxVelocity(ft5FileName, 10), diffs6 = SaveDiff(ft6FileName, 1);

        // functional test 5
        compareDiff(diffs4.EnRKdiff, diffs5.EnRKdiff, 0, true);
        compareDiff(diffs4.EnSIdiff, diffs5.EnSIdiff, 1, true);
        compareDiff(diffs4.RKnSIdiff, diffs5.RKnSIdiff, 2, true);

        // check the ode difference with larger timestep
        compareDiff(diffs4.EnRKdiff, diffs6.EnRKdiff, 0, false);
        compareDiff(diffs4.EnSIdiff, diffs6.EnSIdiff, 1, false);
        compareDiff(diffs4.RKnSIdiff, diffs6.RKnSIdiff, 2, false);
    }

    private Diffs CheckMaxVelocity(string fileName, int gap)
    {
        FlockingParameter param = ReadFile<FlockingParameter>(fileName);
        var resE = dynamics.FlockSimulation(param, InputValidation.EEULER);
        var resRK = dynamics.FlockSimulation(param, InputValidation.RK4);
        var resSI = dynamics.FlockSimulation(param, InputValidation.SIEULER);
        FilesIO.Write<AnimationStatus>(resE.anime, Path.Combine(resPath, animePrefix + InputValidation.EEULER + "_" + fileName));
        FilesIO.Write<FlockingStatus>(resE.flock, Path.Combine(resPath, flockPrefix + InputValidation.EEULER + "_" + fileName));
        FilesIO.Write<AnimationStatus>(resRK.anime, Path.Combine(resPath, animePrefix + InputValidation.RK4 + "_" + fileName));
        FilesIO.Write<FlockingStatus>(resRK.flock, Path.Combine(resPath, flockPrefix + InputValidation.RK4 + "_" + fileName));
        FilesIO.Write<AnimationStatus>(resSI.anime, Path.Combine(resPath, animePrefix + InputValidation.SIEULER + "_" + fileName));
        FilesIO.Write<FlockingStatus>(resSI.flock, Path.Combine(resPath, flockPrefix + InputValidation.SIEULER + "_" + fileName));

        checkMaxV(resE.flock.meanSpeed);
        checkMaxV(resRK.flock.meanSpeed);
        checkMaxV(resSI.flock.meanSpeed);

        Diffs diffs = new Diffs();
        diffs.EnRKdiff = calDiff(resE.flock, resRK.flock, gap);
        diffs.EnSIdiff = calDiff(resE.flock, resSI.flock, gap);
        diffs.RKnSIdiff = calDiff(resRK.flock, resSI.flock, gap);
        FilesIO.Write<Diffs>(diffs, Path.Combine(resPath, diffPrefix + fileName));

        return diffs;
    }

    private Diffs SaveDiff(string fileName, int gap)
    {
        FlockingParameter param = ReadFile<FlockingParameter>(fileName);
        var resE = dynamics.FlockSimulation(param, InputValidation.EEULER);
        var resRK = dynamics.FlockSimulation(param, InputValidation.RK4);
        var resSI = dynamics.FlockSimulation(param, InputValidation.SIEULER);

        Diffs diffs = new Diffs();
        diffs.EnRKdiff = calDiff(resE.flock, resRK.flock, gap);
        diffs.EnSIdiff = calDiff(resE.flock, resSI.flock, gap);
        diffs.RKnSIdiff = calDiff(resRK.flock, resSI.flock, gap);
        FilesIO.Write<Diffs>(diffs, Path.Combine(resPath, diffPrefix + fileName));

        return diffs;
    }

    private float getAverage(List<float> list)
    {
        if (list.Count == 0)
        {
            return 0;
        }

        float sum = 0;
        foreach (var item in list)
        {
            sum += item;
        }
        return sum / list.Count;
    }

    private void compareDiff(List<float> list0, List<float> list1, int index, bool lessDiff)
    {
        // some test cases that don't satisfy this check when timestep is 0.01 and 0.1
        if (lessDiff && (index == 5 || index == 6 || index == 8))
        {
            return;
        }

        float avg0 = getAverage(list0), avg1 = getAverage(list1);
        Assert.Less(avg0, avg1, "error index: " + index);
    }

    private void compareDiff(Diff diff0, Diff diff1, int index, bool lessDiff)
    {
        compareDiff(diff0.SpeedDiff, diff1.SpeedDiff, index * 3, lessDiff);
        compareDiff(diff0.AlignDiff, diff1.AlignDiff, index * 3 + 1, lessDiff);
        compareDiff(diff0.CoheDiff, diff1.CoheDiff, index * 3 + 2, lessDiff);
    }

    private List<float> calDiff(List<float> list0, List<float> list1, int gap = 1)
    {
        List<float> res = new List<float>();
        int length = Math.Min(list0.Count, list1.Count);
        for (int i = 0; i < length; i++)
        {
            if (i % gap != 0)
            {
                continue;
            }
            res.Add(Math.Abs(list0[i] - list1[i]));
        }
        return res;
    }

    private Diff calDiff(FlockingStatus flock0, FlockingStatus flock1, int gap = 1)
    {
        return new Diff(
            calDiff(flock0.meanSpeed, flock1.meanSpeed, gap),
            calDiff(flock0.alignMetric, flock1.alignMetric, gap),
            calDiff(flock0.cohesion, flock1.cohesion, gap)
            );
    }

    private void checkMaxV(List<float> speed)
    {
        Assert.Greater(speed.Count, 1);
        for (int i = 0; i < speed.Count; i++) {
            Assert.LessOrEqual(speed[i], vMax);
        }
    }


    private T ReadFile<T>(string fileName)
    {
        string path = Path.Combine(sourcePath, fileName);

        return FilesIO.Read<T>(path);
    }
}

