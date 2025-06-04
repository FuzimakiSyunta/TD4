using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleCameraMove : MonoBehaviour
{
    public Animator animator;
    public Image fadeImage; // 黒いImageをCanvasに置く（α=0からスタート）

    public float interval = 3f;     // 切り替えサイクル
    public float fadeDuration = 5f; // フェード時間

    private int stateIndex = 0;

    void Start()
    {
        StartCoroutine(LoopRoutine());
    }

    IEnumerator LoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // フェードアウト
            yield return StartCoroutine(FadeTo(1.5f));

            // アニメーション切り替え
            SetNextAnimationFlag();

            // フェードイン
            yield return StartCoroutine(FadeTo(0f));
        }
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

    void SetNextAnimationFlag()
    {
        animator.SetBool("isFirstMove", false);
        animator.SetBool("isSecondMove", false);
        animator.SetBool("isThirdMove", false);

        switch (stateIndex)
        {
            case 0: animator.SetBool("isFirstMove", true); break;
            case 1: animator.SetBool("isSecondMove", true); break;
            case 2: animator.SetBool("isThirdMove", true); break;
        }

        stateIndex = (stateIndex + 1) % 3;
    }
}
