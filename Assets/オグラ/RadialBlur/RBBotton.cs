using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBBotton : MonoBehaviour
{
    //ShaderCameraへの参照を保持する変数
    private ShaderCamera shaderCamera;

    void Start()
    {
        
    }

    void Update()
    {
        //キーボードのOが押されたらShaderCameraの有効/無効を切り替えます。
        if (Input.GetKeyDown(KeyCode.O))
        {
            shaderCamera.enabled = !shaderCamera.enabled;
            Debug.Log("RBBotton: ShaderCameraの有効状態を切り替えました: " + shaderCamera.enabled);
        }
    } 
}