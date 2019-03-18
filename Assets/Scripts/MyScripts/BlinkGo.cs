using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkGo : MonoBehaviour
{
    public GameObject statusPanel,foundPanel;
    public float blinkDuration =.5f;
    public TextMeshProUGUI displayText,instructionText;
    public string[] instructions;

    CanvasGroup canvasGroup;
    


    public static BlinkGo Instance;

    // Start is called before the first frame update
    void Start()
    {
        if(!Instance)
        {
            Instance = this;
        }

        canvasGroup = statusPanel.GetComponent<CanvasGroup>();

        StartCoroutine(BlinkTitle());

    }


    public IEnumerator BlinkTitle()
    {
        int count = 0;
        while (!RulerScript.Instance.isPlaneFound)
        {

            if (count > instructions.Length-1)
                count = 0;
           

            


            LeanTween.alphaCanvas(canvasGroup, 0, blinkDuration);
            yield return new WaitWhile(() => LeanTween.isTweening());

            instructionText.text = instructions[count++];

            LeanTween.alphaCanvas(canvasGroup, 1, blinkDuration);
            yield return new WaitWhile(() => LeanTween.isTweening());

        }

        statusPanel.SetActive(false);
        foundPanel.SetActive(true);

    }




 
    // Update is called once per frame
    void Update()
    {

    }
}
