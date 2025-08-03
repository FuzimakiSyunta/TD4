using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour
{
    public Image[] images; // 対象のImageたち
    public Color highlightColor = new Color(1, 1, 1, 1);
    public Color normalColor = new Color(1, 1, 1, 0.3f);

    public int currentIndex = 0;

    //Joy-Conの左右ボタンが前に押されていたかを記録するフラグ
    private bool wasDPadLeftPressed = false;
    private bool wasDPadRightPressed = false;

    void Start()
    {
        UpdateHighlight();
    }

    void Update()
    {
        //Joy-Conの現在の十字キーの状態を取得
        bool currentDPadLeft = (JCScript.Instance != null) ? JCScript.Instance.LeftDPadLeft : false;
        bool currentDPadRight = (JCScript.Instance != null) ? JCScript.Instance.LeftDPadRight : false;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || (currentDPadLeft && !wasDPadLeftPressed))
        {
            currentIndex = (currentIndex + 1) % images.Length;
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (currentDPadRight && !wasDPadRightPressed))
        {
            currentIndex = (currentIndex - 1 + images.Length) % images.Length;
            UpdateHighlight();
        }
        wasDPadLeftPressed = currentDPadLeft;
        wasDPadRightPressed = currentDPadRight;
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = (i == currentIndex) ? highlightColor : normalColor;
        }
    }

    public int Imageindex()
    {
        return currentIndex;
    }
}
