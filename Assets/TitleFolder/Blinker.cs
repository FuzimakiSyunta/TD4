using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    public float blinkInterval = 0.1f;      // 点滅の間隔
    public int maxBlinks = 6;               // 点滅回数
    public float fadeDuration = 1f;         // フェードアウトの時間

    private Image img;
    private float timer = 0f;
    private int blinkCount = 0;
    private bool isFading = false;
    private float fadeTimer = 0f;
    private Color originalColor;

    void Start()
    {
        img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogWarning("Image コンポーネントが見つかりません");
            enabled = false;
            return;
        }
        originalColor = img.color;
    }

    void Update()
    {
        if (!isFading)
        {
            Blink();
        }
        else
        {
            FadeOut();
        }
    }

    void Blink()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            img.enabled = !img.enabled;
            blinkCount++;
            timer = 0f;

            if (blinkCount >= maxBlinks)
            {
                img.enabled = true; // 最後は表示状態で
                isFading = true;
                fadeTimer = 0f;
            }
        }
    }

    void FadeOut()
    {
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (fadeTimer >= fadeDuration)
        {
            img.enabled = false; // 最後に完全に消す
            enabled = false;     // 処理を止める
        }
    }
}
