using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public bool isPostEffect = true; // �|�X�g�G�t�F�N�g�������邩�ǂ���
    public bool enableBrur = true; // �u���[�������邩�ǂ���


    [SerializeField] private Material effectMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if(enableBrur==true)
        {
            // �u���[��������ꍇ
            // �|�X�g�G�t�F�N�g��������ꍇ
            Graphics.Blit(src, dest, effectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
            return;
        }
      
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enableBrur = false;
        }
    
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            enableBrur = true;
        }

    }
}
