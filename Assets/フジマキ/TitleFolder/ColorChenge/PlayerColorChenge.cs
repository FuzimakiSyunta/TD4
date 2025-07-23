using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMaterialSwitcher : MonoBehaviour
{
    [Header("共通のマテリアルリスト (ScriptableObject)")]
    [SerializeField] private MaterialList materialList;

    [Header("現在の選択インデックス (ScriptableObject)")]
    [SerializeField] private ColorIndex colorIndex;

    [Header("切り替え対象のRenderer")]
    [SerializeField] private Renderer targetRenderer;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // PlayerPrefsからインデックスを読み込んでScriptableObjectに設定
        colorIndex.currentIndex = PlayerPrefs.GetInt("PlayerMaterialIndex", colorIndex.currentIndex);

        ApplyMaterial(colorIndex.currentIndex);
    }


    void Update()
    {
        // 特定のシーン名でのみ切り替えを許可
        if (SceneManager.GetActiveScene().name != "ColorChengeScene") return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToNextMaterial();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToBackMaterial();
        }
    }

    /// <summary>次のマテリアルに切り替え</summary>
    public void SwitchToNextMaterial()
    {
        if (materialList.materials.Length == 0) return;

        colorIndex.currentIndex = (colorIndex.currentIndex + 1) % materialList.materials.Length;
        ApplyMaterial(colorIndex.currentIndex);
        SaveColorIndex();
    }

    /// <summary>前のマテリアルに切り替え</summary>
    public void SwitchToBackMaterial()
    {
        if (materialList.materials.Length == 0) return;

        colorIndex.currentIndex = (colorIndex.currentIndex - 1 + materialList.materials.Length) % materialList.materials.Length;
        ApplyMaterial(colorIndex.currentIndex);
        SaveColorIndex();
    }

    /// <summary>指定インデックスのマテリアルに切り替え</summary>
    public void SwitchToMaterial(int index)
    {
        if (index < 0 || index >= materialList.materials.Length) return;

        colorIndex.currentIndex = index;
        ApplyMaterial(index);
        SaveColorIndex();
    }

    /// <summary>マテリアル適用</summary>
    private void ApplyMaterial(int index)
    {
        if (targetRenderer != null && materialList.materials[index] != null)
        {
            targetRenderer.material = materialList.materials[index];
        }
    }
    void SaveColorIndex()
    {
        PlayerPrefs.SetInt("PlayerMaterialIndex", colorIndex.currentIndex);
        PlayerPrefs.Save();
    }

}
