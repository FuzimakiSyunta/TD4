using UnityEngine;

public class BikeMaterialSwitcher : MonoBehaviour
{
    [Header("バイクのマテリアルリスト")]
    public BikeMaterialList materialList;

    [Header("現在の選択インデックス")]
    public ColorIndex colorIndex;

    [Header("ターゲットRenderer")]
    public Renderer targetRenderer;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // PlayerPrefsからインデックスを復元
        colorIndex.currentIndex = PlayerPrefs.GetInt("BikeMaterialIndex", colorIndex.currentIndex);

        ApplyMaterial(colorIndex.currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) SwitchToNext();
        if (Input.GetKeyDown(KeyCode.DownArrow)) SwitchToBack();
    }

    public void SwitchToNext()
    {
        colorIndex.currentIndex = (colorIndex.currentIndex + 1) % materialList.materials.Length;
        ApplyMaterial(colorIndex.currentIndex);
        SaveColorIndex();
    }

    public void SwitchToBack()
    {
        colorIndex.currentIndex = (colorIndex.currentIndex - 1 + materialList.materials.Length) % materialList.materials.Length;
        ApplyMaterial(colorIndex.currentIndex);
        SaveColorIndex();
    }

    void ApplyMaterial(int index)
    {
        if (targetRenderer && materialList.materials.Length > index)
        {
            targetRenderer.material = materialList.materials[index];
        }
    }

    void SaveColorIndex()
    {
        PlayerPrefs.SetInt("BikeMaterialIndex", colorIndex.currentIndex); // ← プレイヤー用と分ける！
        PlayerPrefs.Save();
    }
}
