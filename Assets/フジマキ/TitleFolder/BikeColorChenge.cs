using UnityEngine;

public class BikeMaterialSwitcher : MonoBehaviour
{
    [Header("�؂�ւ��Ώۂ�Renderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("�g�p����}�e���A���ꗗ")]
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
        //// �e�X�g�p�F�X�y�[�X�L�[�Ő؂�ւ�
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
    /// ���̃}�e���A���ɐ؂�ւ�
    /// </summary>
    public void SwitchToNextMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        ApplyMaterial(currentIndex);
    }
    /// <summary>
    /// �O�̃}�e���A���ɐ؂�ւ�
    /// </summary>
    public void SwitchToBackMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex - 1 + materials.Length) % materials.Length;
        ApplyMaterial(currentIndex);
    }


    /// <summary>
    /// �w��C���f�b�N�X�̃}�e���A���ɐ؂�ւ�
    /// </summary>
    public void SwitchToMaterial(int index)
    {
        if (index < 0 || index >= materials.Length) return;

        currentIndex = index;
        ApplyMaterial(index);
    }

    /// <summary>
    /// �}�e���A���K�p
    /// </summary>
    private void ApplyMaterial(int index)
    {
        if (targetRenderer != null && materials[index] != null)
        {
            targetRenderer.material = materials[index];
        }
    }
}
