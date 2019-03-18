using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject startSphere, endSphere,lengthText;


    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        startSphere.transform.position = lineRenderer.GetPosition(0);
        endSphere.transform.position = lineRenderer.GetPosition(1);
        lengthText.transform.position = (endSphere.transform.position + startSphere.transform.position) / 2;
        
    }
}
