using System.Collections;
using UnityEngine;

public class CutInMove : MonoBehaviour
{
    public RectTransform imageA;
    public RectTransform imageB;
    public RectTransform imageC;

    private Vector2 targetPosition = new Vector2(-2480f, 0f);
    private float speed = 7000f;
    private float delayBetween = 0.07f;

    private Vector2 startPos;

    private bool isPlaying = false;

    void Start()
    {
        startPos = new Vector2(2270f, targetPosition.y);
        ResetPositions();
        StartCoroutine(PlayCutIn());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPlaying)
            {
                StartCoroutine(PlayCutIn());
            }
        }
    }

    void ResetPositions()
    {
        imageA.anchoredPosition = startPos;
        imageB.anchoredPosition = startPos;
        imageC.anchoredPosition = startPos;
    }

    IEnumerator PlayCutIn()
    {
        isPlaying = true;

        ResetPositions();

        StartCoroutine(Slide(imageA, targetPosition));
        yield return new WaitForSeconds(delayBetween);

        StartCoroutine(Slide(imageB, targetPosition));
        yield return new WaitForSeconds(delayBetween);

        yield return StartCoroutine(Slide(imageC, targetPosition));

        isPlaying = false;
    }

    IEnumerator Slide(RectTransform image, Vector2 targetPos)
    {
        while (Vector2.Distance(image.anchoredPosition, targetPos) > 0.1f)
        {
            image.anchoredPosition = Vector2.MoveTowards(
                image.anchoredPosition,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        image.anchoredPosition = targetPos;
    }
}
