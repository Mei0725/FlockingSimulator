using FlockingEntity;
using System;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class ChartDrawer : MonoBehaviour
{
    public GameObject speedChartObject;
    public GameObject alignChartObject;
    public GameObject coheChartObject;
    public GameObject speedDiffChartObject;
    public GameObject alignDiffChartObject;
    public GameObject coheDiffChartObject;

    private LineChart speedChart;
    private LineChart alignChart;
    private LineChart coheChart;
    private LineChart speedDiffChart;
    private LineChart alignDiffChart;
    private LineChart coheDiffChart;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        speedChart = speedChartObject.GetComponent<LineChart>();
        alignChart = alignChartObject.GetComponent<LineChart>();
        coheChart = coheChartObject.GetComponent<LineChart>();
        speedDiffChart = speedDiffChartObject.GetComponent<LineChart>();
        alignDiffChart = alignDiffChartObject.GetComponent<LineChart>();
        coheDiffChart = coheDiffChartObject.GetComponent<LineChart>();

        initChart(speedChart, "Mean Speed Chart");
        initChart(alignChart, "Align Chart");
        initChart(coheChart, "Cohesion Chart");
        initChart(speedDiffChart, "Mean Speed Diff Chart");
        initChart(alignDiffChart, "Align Diff Chart");
        initChart(coheDiffChart, "Cohesion Diff Chart");
    }

    private void initChart(LineChart chart, string title)
    {
        var titleObj = chart.EnsureChartComponent<Title>();
        titleObj.text = title;

        var xAxis = chart.EnsureChartComponent<XAxis>();
        //xAxis.splitNumber = 10;
        xAxis.boundaryGap = true;
        xAxis.type = Axis.AxisType.Value;

        var yAxis = chart.EnsureChartComponent<YAxis>();
        yAxis.type = Axis.AxisType.Value;
        yAxis.axisLabel.show = false;
    }

    public virtual void DrawChart(List<FlockingStatus> matrics)
    {
        clearCharts();

        List<List<float>> speeds = new List<List<float>>();
        List<List<float>> aligns = new List<List<float>>();
        List<List<float>> cohes = new List<List<float>>();
        float t = 0.0f;

        for (int i = 0; i < matrics.Count; i++)
        {
            FlockingStatus flock = matrics[i];
            string serie = "line" + i.ToString();
            t = flock.t;

            Draw(speedChart, flock.meanSpeed, serie, t);
            Draw(alignChart, flock.alignMetric, serie, t);
            Draw(coheChart, flock.cohesion, serie, t);
            speeds.Add(flock.meanSpeed);
            aligns.Add(flock.alignMetric);
            cohes.Add(flock.cohesion);
        }

        DrawDiffCharts(speedDiffChart, speeds, t);
        DrawDiffCharts(alignDiffChart, aligns, t);
        DrawDiffCharts(coheDiffChart, cohes, t);

        refreshCharts();

        Debug.Log("Charts drawn.");
    }

    // Do not support comparing with differnt timestep t.
    private void DrawDiffCharts(LineChart chart, List<List<float>> datas, float t)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            for (int j = i + 1; j < datas.Count; j++)
            {
                var line = chart.AddSerie<Line>("line" + i.ToString() + " vs line" + j.ToString());
                int length = Math.Min(datas[i].Count, datas[j].Count);
                for (int k = 0; k < length; k++)
                {
                    float diff = datas[i][k] - datas[j][k];
                    line.AddXYData(t * k, diff * diff);
                }
            }
        }
    }

    private void Draw(LineChart chart, List<float> vals, string serie, float t)
    {
        var line = chart.AddSerie<Line>(serie);
        for (int i = 0; i < vals.Count; i++)
        {
            line.AddXYData(t * i, vals[i]);
        }
    }

    private void clearCharts()
    {
        //speedChart.ClearData();
        speedChart.RemoveData();
        alignChart.RemoveData();
        coheChart.RemoveData();
        speedDiffChart.RemoveData();
        alignDiffChart.RemoveData();
        coheDiffChart.RemoveData();
    }

    private void refreshCharts()
    {
        speedChart.RefreshChart();
        alignChart.RefreshChart();
        coheChart.RefreshChart();
        speedDiffChart.RefreshChart();
        alignDiffChart.RefreshChart();
        coheDiffChart.RefreshChart();
    }
}
