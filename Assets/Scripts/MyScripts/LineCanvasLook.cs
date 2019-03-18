using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCanvasLook : MonoBehaviour
{
    // Start is called before the first frame update
   
    void Start()
    {

        StartCoroutine(CanvasFaceCam());
       
    }


    public IEnumerator CanvasFaceCam()
    {
        while (true)
        {

            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
           
            

            yield return new WaitForEndOfFrame();
        }




    }

    // Update is called once per frame
    void Update()
    {

    }
}
