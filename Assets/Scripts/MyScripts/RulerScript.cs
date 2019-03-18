using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class RulerScript : MonoBehaviour
{

    public GameObject lineRendererPrefab;
    public GameObject indicator;
    public Text statusText;

    public float[] bagSizes;
    public TextMeshProUGUI bagSizeText;
    public GameObject bagPanel;
    public Button drawButton;

    ARPlaneManager arPlaneManager;
    ARSessionOrigin arSessionOrigin;

    Pose currentPose;


    public bool isPlaneFound;
    public bool isTouchDetectable;

    bool isTouched;



    bool isPoseValid;
    bool isDrawing;
    bool isLineToBeInstantiated;
    bool isLineBegan;
    bool isLinecomplete;

    float totalDistance;
    int lineCount;
    int screenTouchCount;


    public static RulerScript Instance;

    Vector3 initPos, currentlength;

    //
    Vector3 touchStartPos, touchEndPos;
    //

    GameObject line;
    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }

        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        isPoseValid = false;
        isDrawing = false;
        isLineBegan = false;
        isLineToBeInstantiated = false;
        isLinecomplete = false;
        isPlaneFound = false;
        


        screenTouchCount = 0;
        lineCount = 0;
        totalDistance = 0;
        statusText.text = "Plane Not Found";
        indicator.SetActive(false);

        bagPanel.SetActive(false);





    }





    public void SetDrawingStatus()
    {

        //isDrawing = (isDrawing == true) ? false : true;

        if (isDrawing)
        {
            drawButton.GetComponent<Image>().color = Color.yellow;
            isDrawing = false;
            isLineToBeInstantiated = false;
            isLineBegan = false;
            isLinecomplete = true;

        }
        else
        {
            isLinecomplete = false;
            isDrawing = true;
            isLineToBeInstantiated = true;

            //if (lineCount == 4)
            //    CalculatePerimeter();

        }


    }

    public void ResetAll()
    {
        ////List<ARPlane> arPlanes = new List<ARPlane>();
        ////arPlaneManager.GetAllPlanes(arPlanes);

        ////foreach (ARPlane plane in arPlanes)
        ////    plane.gameObject.SetActive(false);

        //GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        //if (lines == null)
        //    statusText.text = "no tags";

        //else
        //{
        //    lineCount = 0;
        //    totalDistance = 0;

        //    statusText.text = lines.Length.ToString() + " deleted";

        //    foreach (GameObject go in lines)
        //        Destroy(go.gameObject);


        //}

        //drawButton.GetComponent<Image>().color = Color.yellow;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void CalculatePerimeter()
    {
        if (lineCount > 0)
        {
            totalDistance = 0;
            GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");

            foreach (GameObject line in lines)
            {

                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                float length = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
                totalDistance += length;

            }
            totalDistance = totalDistance * 100;
            statusText.text = totalDistance.ToString("00#.00");

            if (totalDistance < bagSizes[0])
                bagSizeText.text = "The required bag size is " + "small";
            else if (totalDistance < bagSizes[1])
                bagSizeText.text = "The required bag size is " + "medium";
            else if (totalDistance < bagSizes[2])
                bagSizeText.text = "The required bag size is " + "large";
            else
                bagSizeText.text = "The required bag size is " + " extra large";

            bagPanel.SetActive(true);
        }

    }


    //public IEnumerator PlaneFound()
    //{
    //    BlinkGo.Instance.displayText.text = "Plane Found";
    //    BlinkGo.Instance.blinkDuration = 100;



    //}


    public void PlacePose()
    {
        Vector3 centerPoint = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        List<ARRaycastHit> touchhits = new List<ARRaycastHit>();



        arSessionOrigin.Raycast(centerPoint, hits, TrackableType.Planes);  //planes

        isPoseValid = (hits.Count >= 0);
        // statusText.text = "yay!";

        if (isPoseValid)
        {
            

            if (isTouchDetectable)
            {

                if (Input.touchCount > 0)
                {

                    Touch touch = Input.GetTouch(0);

                    statusText.text = "Touched";
                   




                        if (arSessionOrigin.Raycast(touch.position, touchhits, TrackableType.Planes))
                        {
                            if (screenTouchCount == 0)
                            {

                                touchStartPos = touchhits[0].pose.position;
                                screenTouchCount = 1;
                            }

                            if (screenTouchCount == 1)
                            {
                                touchEndPos = touchhits[0].pose.position;
                                screenTouchCount = 0;
                            }
                            





                        }  //planes


                    

                }


            }









            if (arPlaneManager.planeCount > 0)
                isPlaneFound = true;

            currentPose = hits[0].pose;

            Vector3 camv3 = Camera.current.transform.forward;
            Vector3 camBearing = new Vector3(camv3.x, 0, camv3.z);
            currentPose.rotation = Quaternion.LookRotation(camBearing);

            if (!isDrawing)
            {
                drawButton.GetComponent<Image>().color = Color.green;
                indicator.SetActive(true);

                statusText.text = "Plane Found";

            }



        }
        else
            drawButton.GetComponent<Image>().color = Color.yellow;

        if (drawButton.GetComponent<Image>().color == Color.yellow)
            statusText.text = "Plane Not Found";




        if (isDrawing)
        {


            statusText.text = "Drawing";

            if (isLineToBeInstantiated)
            {
                statusText.text = "Line Live";
                line = Instantiate(lineRendererPrefab);
                lineCount++;

                isLineToBeInstantiated = false;
            }


            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            if (!isLineBegan)
            {
                statusText.text = "Initial Position Set";


                if (isTouchDetectable && screenTouchCount == 0)
                    lineRenderer.SetPosition(0, touchStartPos);
                else if (!isTouchDetectable)
                    lineRenderer.SetPosition(0, hits[0].pose.position);

                List<ARPlane> planes = new List<ARPlane>();
                arPlaneManager.GetAllPlanes(planes);
                foreach (ARPlane plane in planes)
                    plane.gameObject.SetActive(false);


                if (isTouchDetectable && screenTouchCount == 0)
                    initPos = touchStartPos;
                else if (!isTouchDetectable)
                    initPos = hits[0].pose.position;



                isLineBegan = true;


            }


            drawButton.GetComponent<Image>().color = Color.red;

            if (isTouchDetectable && screenTouchCount == 1)
            {
                lineRenderer.SetPosition(1, touchEndPos);
                indicator.transform.position = touchEndPos;
                currentlength = touchEndPos;
                screenTouchCount = 0;

            }

            else if (!isTouchDetectable)
            {

                lineRenderer.SetPosition(1, hits[0].pose.position);
                indicator.transform.position = hits[0].pose.position;
                currentlength = hits[0].pose.position;

            }

            float distance = Vector3.Distance(initPos, currentlength);
            statusText.text = (distance * 100).ToString() + " cm";

            line.transform.GetChild(2).GetComponent<TextMeshPro>().text = (distance * 100).ToString("00#.00") + " cm";
            // line.transform.GetChild(0).transform.position = currentlength;

        }







    }




    public void SetIndicator()
    {

        if (isPoseValid && !isDrawing)
        {
            indicator.SetActive(true);
            indicator.transform.SetPositionAndRotation(currentPose.position, currentPose.rotation);
        }
        else
        {
            //indicator.SetActive(false);
        }




    }




    // Update is called once per frame
    void Update()
    {
        PlacePose();
        SetIndicator();
        if (lineCount == 2 && isLinecomplete)
            CalculatePerimeter();



    }
}
