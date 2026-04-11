using FlockingEntity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public TMP_InputField kaInput;
    public TMP_InputField kcInput;
    public TMP_InputField ksInput;
    public TMP_InputField koInput;
    public TMP_InputField kgInput;
    public TMP_InputField tInput;
    public TMP_InputField nInput;
    public TMP_InputField obsInput;
    public TMP_Dropdown methodInput;
    public TMP_InputField initPathInput;
    public TMP_InputField animaPathInput;
    public TMP_InputField flockPathInput;

    public virtual FlockingParameter GetUserInput()
    {
        FlockingParameter flock = new FlockingParameter();

        string ka = kaInput.text;
        flock.ka = InputValidation.KVerify(ka, InputValidation.InputType.KA);
        string kc = kcInput.text;
        flock.kc = InputValidation.KVerify(kc, InputValidation.InputType.KC);
        string ks = ksInput.text;
        flock.ks = InputValidation.KVerify(ks, InputValidation.InputType.KS);
        string ko = koInput.text;
        flock.ko = InputValidation.KVerify(ko, InputValidation.InputType.KO);
        string kg = kgInput.text;
        flock.kg = InputValidation.KVerify(kg, InputValidation.InputType.KG);

        string t = tInput.text;
        flock.t = InputValidation.TVerify(t);

        string n = nInput.text;
        flock.n = InputValidation.NVerify(n);

        string obs = obsInput.text;
        flock.obs = InputValidation.ObsVerify(obs);

        return flock;
    }

    public virtual FlockingParameter GetSimulateParam()
    {
        FlockingParameter flock = new FlockingParameter();
        string path = initPathInput.text;
        InputValidation.PathVerify(path);

        return FilesIO.Read<FlockingParameter>(path);
    }

    public virtual string GetMethod()
    {
        int index = methodInput.value;
        string method = "";
        switch (index)
        {
            case 0:
                method = InputValidation.EEULER;
                break;
            case 1:
                method = InputValidation.RK4;
                break;
            case 2:
                method = InputValidation.SIEULER;
                break;
            default:
                Debug.Log("Error ODE method:" + index);
                break;

        }
        InputValidation.MethodVerify(method);
        return method;
    }

    public virtual List<AnimationStatus> GetAnimaStatus()
    {
        List<AnimationStatus> res = new List<AnimationStatus>();

        string input = animaPathInput.text;
        string[] paths = input.Split(';');
        foreach (string path in  paths)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                continue;
            }
            res.Add(FilesIO.Read<AnimationStatus>(path));
        }

        return res;
    }

    public virtual List<FlockingStatus> GetFlockStatus()
    {
        List<FlockingStatus> res = new List<FlockingStatus>();

        string input = flockPathInput.text;
        string[] paths = input.Split(';');
        foreach (string path in paths)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                continue;
            }
            res.Add(FilesIO.Read<FlockingStatus>(path));
        }
        return res;
    }
}