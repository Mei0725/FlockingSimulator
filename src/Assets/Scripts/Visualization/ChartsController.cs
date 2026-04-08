using System.Collections.Generic;
using UnityEngine;

public class ChartsController : MonoBehaviour
{
    public GameObject controller;
    public GameObject charts;

    public List<GameObject> chartObjects;

    private int isDiff = 0;
    private int chartIndex = 0;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        controller.SetActive(true);
        charts.SetActive(false);
    }

    public void OnDrawButton()
    {
        controller.SetActive(false);
        charts.SetActive(true);
    }

    public void OnBackButton()
    {
        controller.SetActive(true);
        charts.SetActive(false);
    }

    public void OnSpeedClick()
    {
        chartIndex = 0;
        if (isDiff == 0)
        {
            SetActive(0);
        } else
        {
            SetActive(1);
        }
    }

    public void OnAlignClick()
    {
        chartIndex = 1;
        if (isDiff == 0)
        {
            SetActive(2);
        }
        else
        {
            SetActive(3);
        }
    }

    public void OnCoheClick()
    {
        chartIndex = 2;
        if (isDiff == 0)
        {
            SetActive(4);
        }
        else
        {
            SetActive(5);
        }
    }

    public void OnOverViewClick()
    {
        isDiff = 0;
        if (chartIndex == 0)
        {
            SetActive(0);
        }
        else if (chartIndex == 1)
        {
            SetActive(2);
        }
        else if (chartIndex == 2)
        {
            SetActive(4);
        }
    }

    public void OnDiffClick()
    {
        isDiff = 1;
        if (chartIndex == 0)
        {
            SetActive(1);
        }
        else if (chartIndex == 1)
        {
            SetActive(3);
        }
        else if (chartIndex == 2)
        {
            SetActive(5);
        }
    }

    private void SetActive(int index)
    {
        for (int i = 0; i < chartObjects.Count; i++)
        {
            if (index == i)
            {
                chartObjects[i].SetActive(true);
            } else
            {
                chartObjects[i].SetActive(false);
            }
        }
    }
}
