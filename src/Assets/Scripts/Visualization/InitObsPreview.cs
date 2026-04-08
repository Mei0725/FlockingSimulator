using FlockingEntity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitObsPreview : MonoBehaviour
{
    public Transform envBackground;
    public GameObject obsPrefab; 
    public TMP_InputField obsInput;

    private List<GameObject> currentObs = new List<GameObject>(); 
    private float scaleFactor = 0.8f;
    private float worldSize = 500f;
    private float baseSize = 100f;

    public void OnPreviewClick()
    {
        foreach (GameObject obs in currentObs)
        {
            Destroy(obs);
        }
        currentObs.Clear();

        List<Obstacle> input = InputValidation.ObsVerify(obsInput.text);
        foreach (Obstacle ob in input) {
            GameObject currentInstance = Instantiate(obsPrefab, envBackground);

            float x = (ob.p.x - worldSize / 2f) * scaleFactor;
            float y = (ob.p.y - worldSize / 2f) * scaleFactor;

            currentInstance.transform.localPosition = new Vector3(x, y, 0);

            float uiDiameter = 2f * ob.r * scaleFactor;
            float scale = uiDiameter / baseSize;

            currentInstance.transform.localScale = new Vector3(scale, scale, 1f);
            currentObs.Add(currentInstance);
        }
    }
}
