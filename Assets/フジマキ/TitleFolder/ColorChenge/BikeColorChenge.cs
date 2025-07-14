using UnityEngine;

public class BikeMaterialSwitcher : MonoBehaviour
{
    [Header("切り替え対象のRenderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("使用するマテリアル一覧")]
    [SerializeField] private Material[] materials;

    private int currentIndex = 0;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        ApplyMaterial(currentIndex);
    }

    void Update()
    {
        //// テスト用：スペースキーで切り替え
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SwitchToNextMaterial();
        //}

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchToNextMaterial();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchToBackMaterial();
        }
    }

    /// <summary>
    /// 次のマテリアルに切り替え
    /// </summary>
    public void SwitchToNextMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        ApplyMaterial(currentIndex);
    }
    /// <summary>
    /// 前のマテリアルに切り替え
    /// </summary>
    public void SwitchToBackMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex - 1 + materials.Length) % materials.Length;
        ApplyMaterial(currentIndex);
    }


    /// <summary>
    /// 指定インデックスのマテリアルに切り替え
    /// </summary>
    public void SwitchToMaterial(int index)
    {
        if (index < 0 || index >= materials.Length) return;

        currentIndex = index;
        ApplyMaterial(index);
    }

    /// <summary>
    /// マテリアル適用
    /// </summary>
    private void ApplyMaterial(int index)
    {
        if (targetRenderer != null && materials[index] != null)
        {
            targetRenderer.material = materials[index];
        }
    }
}
