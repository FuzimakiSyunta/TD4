using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour
{
    public Image[] images; // ëŒè€ÇÃImageÇΩÇø
    public Color highlightColor = new Color(1, 1, 1, 1);
    public Color normalColor = new Color(1, 1, 1, 0.3f);

    private int currentIndex = 0;

    void Start()
    {
        UpdateHighlight();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex + 1) % images.Length;
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex - 1 + images.Length) % images.Length;
            UpdateHighlight();
        }
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
