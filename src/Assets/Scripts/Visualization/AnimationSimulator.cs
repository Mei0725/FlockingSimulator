using FlockingEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSimulator : MonoBehaviour
{
    public GameObject cam;
    public GameObject animeUI;
    public GameObject labels;
    public Material obsMaterial;

    private CameraManager manager;
    private int playing = 0, center = 0;
    private List<GameObject> obstacles;

    private void Start()
    {
        init();
    }

    public void init()
    {
        manager = cam.GetComponent<CameraManager>();
        playing = 0;
    }

    public virtual void PlaySimulation(AnimationStatus anime)
    {
        if (playing > 0)
        {
            Debug.LogError("Can't play animation when other one is playing.");
            return;
        }

        StopAllCoroutines();
        animeUI.SetActive(false);
        //labels.SetActive(true);
        DrawObstacles(anime.obs);

        playing = 1;
        center = 0;
        StartCoroutine(Simulation(anime, 0, new Color(-1, -1, -1)));
    }

    public virtual void PlaySimulation(List<AnimationStatus> animes)
    {
        if (playing > 0)
        {
            Debug.LogError("Can't play animation when other one is playing.");
            return;
        }

        StopAllCoroutines();
        animeUI.SetActive(false);
        if (animes.Count > 1)
        {
            labels.SetActive(true);
        }
        DrawObstacles(animes[0].obs);

        int simu = animes.Count;
        float colorChange = 1.0f / (simu - 1);
        playing = simu;
        center = 0;
        for (int i = 0; i < simu; i++)
        {
            Color color = new Color(colorChange * i, colorChange * i, colorChange * i);
            StartCoroutine(Simulation(animes[i], i, color));
        }
    }

    private void DrawObstacles(List<Obstacle> obs)
    {
        Debug.Log("Draw obstacles");
        obstacles = new List<GameObject>();

        foreach (Obstacle o in obs)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material = obsMaterial;

            obj.transform.position = new Vector3(o.p.x, o.p.y, 0f);
            float diameter = 2f * o.r;
            obj.transform.localScale = new Vector3(diameter, diameter, 1f);
            obstacles.Add(obj);
        }
    }

    private void DestroyObs()
    {
        foreach (GameObject obs in obstacles)
        {
            Destroy(obs);
        }
    }

    private IEnumerator Simulation(AnimationStatus anime, int index, Color color)
    {
        if (anime == null || anime.agents == null || anime.agents.Count <= 0)
        {
            animeUI.SetActive(true);
            labels.SetActive(false);
            yield break;
        }

        GameObject parent = new GameObject("Line" + index.ToString() + "Agents");
        List<GameObject> objs = new List<GameObject>();
        int length = anime.agents.Count, num = anime.agents[0].positions.Count;

        for (int i = 0; i < num; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.SetParent(parent.transform);
            obj.name = "Agent" + index.ToString() + "_ " + i.ToString();
            obj.tag = "Agent";
            if (color.r >= 0)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_BaseColor", color);
                }
            }
            objs.Add(obj);
        }

        if (center == index && manager != null) manager.getAgents("Agent" + index.ToString());

        float t = anime.t;
        for (int i = 0; i < length; i++)
        {
            List<V3> status = anime.agents[i].positions;
            for (int j = 0; j < num; j++)
            {
                objs[j].transform.position = status[j].ToVector3();
            }
            if (manager != null) manager.UpdateCameraCenter();
            yield return new WaitForSeconds(t);
        }
        Debug.Log("Simulation playback finished.");

        // Destory
        if (center == index && manager != null) manager.DestroyAgents();
        //foreach (GameObject obj in objs)
        //{
        //    Destroy(obj);
        //}
        Destroy(parent);
        playing--;

        animeUI.SetActive(true);
        labels.SetActive(false);
        DestroyObs();
    }
}
