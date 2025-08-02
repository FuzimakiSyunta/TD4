using UnityEngine;
using UnityEngine.SceneManagement;

public class BikeMaterialSwitcher : MonoBehaviour
{
    [Header("バイクのマテリアルリスト")]
    public BikeMaterialList materialList;

    [Header("現在の選択インデックス")]
    public ColorIndex colorIndex;

    [Header("ターゲットRenderer")]
    public Renderer targetRenderer;

    [Header("カラーチェンジを許可するシーン名（カンマ区切り）")]
    public string[] allowedScenes = { "ColorChengeScene" };

    public PlayerColorChenge playerColorChengeScript; // ← 追加: プレイヤー用のスクリプトを参照
    public GameObject playerColorChenge;

    public GameObject redArrow_Right;
    public GameObject redArrow_Left;

    public GameObject bikeIcon; // バイクアイコンの参照

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // PlayerColorChengeスクリプトの参照を取得
        if (playerColorChenge != null)
        {
            playerColorChengeScript = playerColorChenge.GetComponent<PlayerColorChenge>();
        }
    

        // PlayerPrefsからインデックスを復元
        colorIndex.currentIndex = PlayerPrefs.GetInt("BikeMaterialIndex", colorIndex.currentIndex);

        ApplyMaterial(colorIndex.currentIndex);
    }

    void Update()
    {
        if (!IsSceneAllowed() || !playerColorChengeScript.IsSelected()) return;

        // →キーを押した瞬間
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToNext();
            redArrow_Right.SetActive(true);
            redArrow_Left.SetActive(false);
        }
        // ←キーを押した瞬間
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToBack();
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

        // バイクアイコン表示制御
        if (playerColorChengeScript.IsSelected())
        {
            bikeIcon.SetActive(true);
        }
        else
        {
            bikeIcon.SetActive(false);
        }
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
