using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChenge : MonoBehaviour
{
    public Material[] colorMaterials;
    private int count;
    private bool canInput = true;  // ���͉\���ǂ����������t���O
    private float previousDph = 0; // �O���D_Pad_H�̒l
    private float previousDpv = 0; // �O���D_Pad_V�̒l
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
                // �K�v�ɉ����ĉ����̏�����ǉ�
                canInput = false;
            }

            if (dpv < 0 && previousDpv >= 0)
            {
                // �K�v�ɉ����ĉ����̏�����ǉ�
                canInput = false;
            }
        }

        if (dph == 0 && dpv == 0)
        {
            canInput = true;
        }

        previousDph = dph;
        previousDpv = dpv;
        // �}�e���A���̖��O���`�F�b�N���ă��j���[��؂�ւ���
        Material currentMaterial = colorMaterials[count];
        if (currentMaterial.name.Contains("") || currentMaterial.name.Contains("") || currentMaterial.name.Contains(""))
        {
            
        }
        else if (currentMaterial.name.Contains("") || currentMaterial.name.Contains("") || currentMaterial.name.Contains(""))
        {
            
        }
    }
}
