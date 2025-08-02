using UnityEngine;

public class UIShineController : MonoBehaviour
{
    [SerializeField] private Material uiMaterial;

    public void SetAlpha(float alpha)
    {
        uiMaterial.SetFloat("_AlphaControl", Mathf.Clamp01(alpha));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeRoutine(duration));
    }

    private System.Collections.IEnumerator FadeRoutine(float duration)
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime / duration;
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(0f);
    }
}
