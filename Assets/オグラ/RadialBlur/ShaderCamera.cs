using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ShaderCamera : MonoBehaviour
{
    public Material blurMaterial;

    [Range(0.0f, 1.0f)]
    public float blurStrength = 0.5f;

    [Range(1, 64)]
    public int blurSamples = 16;

    public Vector2 blurCenter = new Vector2(0.5f, 0.5f);

    private void Update()
    {
    
    }

    void OnEnable()
    {
        //この条件がtrueになると、スクリプトが無効になります
        if (blurMaterial == null || blurMaterial.shader == null || !blurMaterial.shader.isSupported)
        {
            enabled = true;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurMaterial == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        blurMaterial.SetFloat("_BlurStrength", blurStrength);
        blurMaterial.SetInt("_BlurSamples", blurSamples);
        blurMaterial.SetVector("_BlurCenter", blurCenter);

        Graphics.Blit(source, destination, blurMaterial);
    }
}