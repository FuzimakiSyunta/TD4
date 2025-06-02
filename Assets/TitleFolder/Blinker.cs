using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    public float blinkInterval = 0.1f;      // �_�ł̊Ԋu
    public int maxBlinks = 6;               // �_�ŉ�
    public float fadeDuration = 1f;         // �t�F�[�h�A�E�g�̎���

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
            Debug.LogWarning("Image �R���|�[�l���g��������܂���");
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
                img.enabled = true; // �Ō�͕\����Ԃ�
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
            img.enabled = false; // �Ō�Ɋ��S�ɏ���
            enabled = false;     // �������~�߂�
        }
    }
}
