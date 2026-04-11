using FlockingEntity;
using MyExceptions;
using System.Collections.Generic;
using UnityEngine;

public class Dynamics
{
    //private Vector3 GOAL_POINT = new Vector3(100, 100, 0);
    private Vector3 GOAL_POINT = new Vector3(500, 500, 0);
    private float maxTime = 1000f;
    private float arrive = 5.0f;

    public virtual (AnimationStatus anime, FlockingStatus flock) FlockSimulation(FlockingParameter initVal, string method)
    {
        FlockingStatus flock = new FlockingStatus
        {
            t = initVal.t
        };
        List<Vector3> velos = new List<Vector3>();
        List<Vector3> positions = new List<Vector3>();

        AnimationStatus anime = new AnimationStatus
        {
            t = initVal.t,
            obs = initVal.obs
        };

        foreach (V3 point in initVal.starts)
        {
            velos.Add(new Vector3(0, 0, 0));
            positions.Add(point.ToVector3());
        }
        addPositionToInfo(anime, positions);

        // init line charts data
        flock.meanSpeed.Add(FlockingMetrics.CalMeanSpeed(velos));
        flock.alignMetric.Add(FlockingMetrics.CalAlignMetric(velos));
        flock.cohesion.Add(FlockingMetrics.CalCohesion(positions));

        float totalTime = 0;
        while (totalTime <= maxTime)
        {
            List<Vector3>[] curInfo = ODEChoose(method, positions, velos, initVal);
            velos = curInfo[0];
            positions = curInfo[1];
            addPositionToInfo(anime, positions);

            //calculate status
            Vector3 center = FlockingMetrics.CalCenter(positions);
            flock.meanSpeed.Add(FlockingMetrics.CalMeanSpeed(velos));
            flock.alignMetric.Add(FlockingMetrics.CalAlignMetric(velos));
            flock.cohesion.Add(FlockingMetrics.CalCohesion(center, positions));
            //Debug.Log("time: " + totalTime + ", center: " + center);

            totalTime += initVal.t;
            if (Vector3.Distance(GOAL_POINT, center) < arrive)
            {
                Debug.Log("Arrive at goal point.");
                break;
            }
        }

        Debug.Log("Simulation stops. Spend time: " + totalTime);
        return (anime, flock);
    }

    private List<Vector3>[] ODEChoose(string method, List<Vector3> ps, List<Vector3> vs, FlockingParameter info)
    {
        List<Vector3>[] result = null;
        switch (method)
        {
            case InputValidation.EEULER:
                result = ODESolver.EEulerMethod(ps, vs, info, GOAL_POINT);
                break;
            case InputValidation.RK4:
                result = ODESolver.RK4Method(ps, vs, info, GOAL_POINT);
                break;
            case InputValidation.SIEULER:
                result = ODESolver.SIEulerMethod(ps, vs, info, GOAL_POINT);
                break;
            default:
                Debug.LogWarningFormat("Wrong method: " + method);
                throw new ErrorMethod();
        }
        return result;
    }

    private void addPositionToInfo(AnimationStatus anime, List<Vector3> positions)
    {
        Agents agents = new Agents();
        for (int i = 0; i < positions.Count; i++)
        {
            agents.positions.Add(new V3(positions[i]));
        }
        anime.agents.Add(agents);
    }
}
