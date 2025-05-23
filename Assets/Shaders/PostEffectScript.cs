using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public bool isPostEffect = true; // ポストエフェクトをかけるかどうか
    public bool enableBrur = true; // ブラーをかけるかどうか


    [SerializeField] private Material effectMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if(enableBrur==true)
        {
            // ブラーをかける場合
            // ポストエフェクトをかける場合
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
