using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{

    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        LeanTween.alphaCanvas(canvasGroup, 1, .5f);

        yield return new WaitWhile(() => LeanTween.isTweening(transform.gameObject));

        LeanTween.alphaCanvas(canvasGroup, 0, 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
