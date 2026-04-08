using FlockingEntity;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public GameObject controller;

    public TMP_Text initResult;
    public TMP_Text simuResult;
    public TMP_Text animeResult;
    public TMP_Text chartResult;

    protected InputReader inputReader;
    protected AnimationSimulator animationSimulator;
    protected ChartDrawer chartDrawer;
    private Dynamics dynamics = new Dynamics();

    private float startRadius = 5.0f;

    private string prefixPath;
    private string initPrefix = "init_";
    private string animePrefix = "anime_";
    private string flockPrefix = "flock_";
    private string saveResult = "The result has been saved at ";

    void Awake()
    {
        AwakeInit();
    }

    public void AwakeInit()
    {
        prefixPath = Application.persistentDataPath;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        animationSimulator = controller.GetComponent<AnimationSimulator>();
        chartDrawer = controller.GetComponent<ChartDrawer>();
        inputReader = controller.GetComponent<InputReader>();
    }

    public void InitClicked()
    {
        try
        {
            FlockingParameter param = inputReader.GetUserInput();

            FlockingInit(param);

            string path = saveSimulationData(param, initPrefix);

            initResult.color = Color.green;
            initResult.text = saveResult + path;
        } catch (Exception e)
        {
            initResult.color = Color.red;
            initResult.text = e.Message;
        }
    }

    private void FlockingInit(FlockingParameter param)
    {
        List<V3> starts = new List<V3>();
        for (int i = 0; i < param.n; i++)
        {
            Vector2 point2D = UnityEngine.Random.insideUnitCircle * startRadius;
            starts.Add(new V3(point2D.x, point2D.y, 0));
        }
        param.starts = starts;
    }

    public void CalculateClicked()
    {
        try
        {
            FlockingParameter info = inputReader.GetSimulateParam();
            string method = inputReader.GetMethod();
            var res = dynamics.FlockSimulation(info, method);

            // save data
            string animeFile = saveSimulationData(res.anime, animePrefix, method);
            string flockFile = saveSimulationData(res.flock, flockPrefix, method);

            // show result
            simuResult.color = Color.green;
            simuResult.text = saveResult + animeFile + ";" + flockFile;
        } catch(Exception e)
        {
            simuResult.color = Color.red;
            simuResult.text = e.Message;
            Debug.LogWarning(e);
        }
    }

    public void PlayAnimaClicked()
    {
        try
        {
            List<AnimationStatus> animes = inputReader.GetAnimaStatus();
            //animationSimulator.PlaySimulation(animes[0]);
            animationSimulator.PlaySimulation(animes);
        }
        catch (Exception e)
        {
            animeResult.color = Color.red;
            animeResult.text = e.Message;
        }
    }

    public void DrawClicked()
    {
        try
        {
            List<FlockingStatus> flockings = inputReader.GetFlockStatus();
            chartDrawer.DrawChart(flockings);
        }
        catch (Exception e)
        {
            chartResult.color = Color.red;
            chartResult.text = e.Message;
        }
    }

    private string saveSimulationData<T>(T contents, string prefix)
    {
        string fileName = prefix + Guid.NewGuid().ToString() + ".json";
        string path = Path.Combine(prefixPath, fileName);
        FilesIO.Write<T>(contents, path);
        return path;
    }

    private string saveSimulationData<T>(T contents, string prefix, string method)
    {
        string fileName = prefix + method + "_" + Guid.NewGuid().ToString() + ".json";
        string path = Path.Combine(prefixPath, fileName);
        FilesIO.Write<T>(contents, path);
        return path;
    }

    public void InjectDependencies(InputReader i, AnimationSimulator a, ChartDrawer c, Dynamics d)
    {
        inputReader = i;
        animationSimulator = a;
        chartDrawer = c;
        dynamics = d;
    }
}