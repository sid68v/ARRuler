using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrail : MonoBehaviour
{

    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount >0)
        {

            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began && touch.phase ==TouchPhase.Moved)
            {

             trailRenderer.AddPosition(touch.position);


            }

            if(touch.phase ==TouchPhase.Ended)
            {
                trailRenderer.Clear();

            }

            


        }

        
    }
}
