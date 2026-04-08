using UnityEngine;
using System.Linq;

public class CameraManager : MonoBehaviour
{
    public float smoothSpeed = 0.5f;   // move speed of camera
    public Vector3 offset;           // the movement between camera and center

    public float zoomSmooth = 5f;

    public float zoomMultiplier = 2f;
    public float minZoom = 20f;
    public float maxZoom = 60f;

    private Vector3 moveVelocity;
    private float zoomVelocity;

    private GameObject[] agents;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void getAgents(string prefix)
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Agent");
        agents = allAgents
            .Where(obj => obj != null && obj.name.StartsWith(prefix))
            .ToArray();
    }

    public void DestroyAgents()
    {
        agents = null;
    }

    public void UpdateCameraCenter()
    {
        //Debug.Log("camera changes.");

        if (agents == null || agents.Length == 0)
            return;

        Bounds bounds = new Bounds(agents[0].transform.position, Vector3.zero);
        foreach (GameObject agent in agents)
        {
            bounds.Encapsulate(agent.transform.position);
        }

        // flock center
        Vector3 center = bounds.center;
        // flock radius
        float radius = bounds.extents.magnitude;

        // position of camera
        Vector3 targetPosition = center + offset;
        // move camera
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref moveVelocity,
            smoothSpeed * Time.deltaTime
        );

        // scale camera
        float targetZoom = Mathf.Clamp(
            radius * zoomMultiplier,
            minZoom,
            maxZoom
        );
        cam.orthographicSize = Mathf.SmoothDamp(
            cam.orthographicSize,
            targetZoom,
            ref zoomVelocity,
            zoomSmooth * Time.deltaTime
        );
    }
}
