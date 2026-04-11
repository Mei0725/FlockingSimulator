using FlockingEntity;
using MyExceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class InputValidation
{
    public const string EEULER = "EEULER";
    public const string RK4 = "RK4";
    public const string SIEULER = "SIEuler";

    public const string ALIGNMENT = "Alignment";
    public const string COHESION = "Cohesion";
    public const string SEPARATION = "Separation";
    public const string OBSAVOID = "Obs Avoid";
    public const string GOALSEEK = "Goal Seek";

    private static float kMin = 0;
    private static float kMax = 2.0f;
    private static int NMin = 1;
    private static int NMax = 5000;
    private static float tMin = 0.005f;
    private static float tMax = 0.1f;
    private static float pMin = -500;
    private static float pMax = 1000;
    private static float rMin = 1;
    private static float rMax = 500;
    private static string pattern = @"-?\d+(\.\d+)?";
    private static HashSet<string> methods = new HashSet<string> { EEULER, RK4, SIEULER };

    public enum InputType
    {
        KA,
        KS,
        KC,
        KO,
        KG,
        T,
        N,
        R,
        P
    }

    public static float KVerify(string input, InputType type)
    {
        return ValidateInput(input, kMin, kMax, type);
    }

    public static int NVerify(string input)
    {
        return ValidateInput(input, NMin, NMax, InputType.N);
    }

    public static float TVerify(string input)
    {
        return ValidateInput(input, tMin, tMax, InputType.T);
    }

    public static List<Obstacle> ObsVerify(string input)
    {
        List<Obstacle> res = new List<Obstacle>();

        string[] blocks = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string obs in blocks)
        {
            string block = obs.Trim();

            if (string.IsNullOrEmpty(block))
                continue;

            Obstacle obstacle = new Obstacle();

            MatchCollection nums = Regex.Matches(block, pattern);

            if (nums.Count < 3)
            {
                if (obs.Contains("("))
                {
                    throw new MissingR(obs);
                } else
                {
                    throw new MissingP(obs);
                }
            }
            obstacle.p = new V3(ValidateInput(nums[0].Value, pMin, pMax, InputType.P), ValidateInput(nums[1].Value, pMin, pMax, InputType.P), 0);
            obstacle.r = ValidateInput(nums[2].Value, rMin, rMax, InputType.R) ;

            res.Add(obstacle);
        }
        return res;
    }

    public static void MethodVerify(string method)
    {
        if (string.IsNullOrWhiteSpace(method))
        {
            throw new MissingMethod();
        } else if (!methods.Contains(method))
        {
            throw new ErrorMethod();
        }
    }

    public static void PathVerify(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new MissingPath();
        }
    }

    private static float ValidateInput(string input, float min, float max, InputType type)
    {
        float value = 0;
        if (string.IsNullOrWhiteSpace(input))
        {
            ThrowMissingException(type);
        }
        else if (!float.TryParse(input, out value) || value < min || value > max)
        {
            ThrowErrorException(input, type);
        }
        return value;
    }

    private static int ValidateInput(string input, int min, int max, InputType type)
    {
        int value = 0;
        if (string.IsNullOrWhiteSpace(input))
        {
            ThrowMissingException(type);
        }
        else if (!int.TryParse(input, out value) || value < min || value > max)
        {
            ThrowErrorException(input, type);
        }
        return value;
    }

    private static void ThrowMissingException(InputType type)
    {
        switch (type)
        {
            case InputType.KA:
                throw new MissingK(ALIGNMENT);
            case InputType.KS:
                throw new MissingK(SEPARATION);
            case InputType.KC:
                throw new MissingK(COHESION);
            case InputType.KO:
                throw new MissingK(OBSAVOID);
            case InputType.KG:
                throw new MissingK(GOALSEEK);
            case InputType.N:
                throw new MissingN();
            case InputType.T:
                throw new MissingT();
            case InputType.R:
                throw new MissingR();
            case InputType.P:
                throw new MissingP();
            default:
                return;
        }
    }

    private static void ThrowErrorException(string input, InputType type)
    {
        switch (type)
        {
            case InputType.KA:
                throw new ErrorK(ALIGNMENT, kMin, kMax);
            case InputType.KS:
                throw new ErrorK(SEPARATION, kMin, kMax);
            case InputType.KC:
                throw new ErrorK(COHESION, kMin, kMax);
            case InputType.KO:
                throw new ErrorK(OBSAVOID, kMin, kMax);
            case InputType.KG:
                throw new ErrorK(GOALSEEK, kMin, kMax);
            case InputType.N:
                throw new ErrorN(NMin, NMax);
            case InputType.T:
                throw new ErrorT(tMin, tMax);
            case InputType.R:
                throw new ErrorR(rMin, rMax);
            case InputType.P:
                throw new ErrorP(pMin, pMax);
            default:
                return;
        }
    }

}
