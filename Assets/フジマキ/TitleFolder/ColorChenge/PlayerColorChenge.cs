using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerColorChenge : MonoBehaviour
{
    [Header("共通のマテリアルリスト (ScriptableObject)")]
    [SerializeField] private MaterialList materialList;

    [Header("現在の選択インデックス (ScriptableObject)")]
    [SerializeField] private ColorIndex colorIndex;

    [Header("切り替え対象のRenderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("カラーチェンジを許可するシーン名（カンマ区切り）")]
    public string[] allowedScenes = { "ColorChengeScene" };

    public bool isSelected = false; // ← 追加: 選択状態を管理するフラグ

    public GameObject redArrow_Right;
    public GameObject redArrow_Left;

    public float aroowActiveTime = 0.0f; // ← 追加: 矢印のアクティブ時間

    public GameObject playerIcon;

    // キーが押されたかどうかのフラグ
    private bool hasPressedEnter = false;
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
        if (!IsSceneAllowed() || IsSelected()) return; // ← 現在のシーンが対象外なら入力を無視

        // →キーが押された瞬間
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToNextMaterial();
            redArrow_Right.SetActive(true);
            redArrow_Left.SetActive(false);
        }

        // ←キーが押された瞬間
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToBackMaterial();
            redArrow_Left.SetActive(true);
            redArrow_Right.SetActive(false);
        }

        // →キーを離した瞬間
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            redArrow_Right.SetActive(false);
        }

        // ←キーを離した瞬間
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            redArrow_Left.SetActive(false);
        }

        // プレイヤーアイコンの表示切り替え
        if (!isSelected)
        {
            playerIcon.SetActive(true);
        }
        else
        {
            playerIcon.SetActive(false);
        }

        Selected();
    }


    bool IsSceneAllowed() // ← 追加: シーン名が許可されているか確認
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (string scene in allowedScenes)
        {
            if (scene == currentScene) return true;
        }
        return false;
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

    // ↓これで選択後はもう1回押されるまで反応しない
    void Selected()
    {
        if (!hasPressedEnter && Input.GetKeyDown(KeyCode.Return))
        {
            hasPressedEnter = true;
            isSelected = true;
        }
    }


    public bool IsSelected()
    {
        return isSelected;
    }

   
    
}
