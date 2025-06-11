using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChenge : MonoBehaviour
{
    public Material[] colorMaterials;
    private int count;
    private bool canInput = true;  // 入力可能かどうかを示すフラグ
    private float previousDph = 0; // 前回のD_Pad_Hの値
    private float previousDpv = 0; // 前回のD_Pad_Vの値
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dph = Input.GetAxis("D_Pad_H");
        float dpv = Input.GetAxis("D_Pad_V");
        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || dph > 0 && previousDph >= 0)
            {
                count++;
                if (count > colorMaterials.Length - 1)
                {
                    count = 0;
                }
                GetComponent<MeshRenderer>().material = colorMaterials[count];
                canInput = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || dph < 0 && previousDph <= 0)
            {
                count--;
                if (count < 0)
                {
                    count = colorMaterials.Length - 1;
                }
                GetComponent<MeshRenderer>().material = colorMaterials[count];
                canInput = false;
            }

            if (dpv > 0 && previousDpv <= 0)
            {
                // 必要に応じて何かの処理を追加
                canInput = false;
            }

            if (dpv < 0 && previousDpv >= 0)
            {
                // 必要に応じて何かの処理を追加
                canInput = false;
            }
        }

        if (dph == 0 && dpv == 0)
        {
            canInput = true;
        }

        previousDph = dph;
        previousDpv = dpv;
        // マテリアルの名前をチェックしてメニューを切り替える
        Material currentMaterial = colorMaterials[count];
        if (currentMaterial.name.Contains("") || currentMaterial.name.Contains("") || currentMaterial.name.Contains(""))
        {
            
        }
        else if (currentMaterial.name.Contains("") || currentMaterial.name.Contains("") || currentMaterial.name.Contains(""))
        {
            
        }
    }
}
