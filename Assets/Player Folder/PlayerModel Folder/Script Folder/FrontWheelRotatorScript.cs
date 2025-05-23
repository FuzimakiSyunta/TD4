using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheelRotatorScript : MonoBehaviour
{
    // �v���C���[�̑O�փI�u�W�F�N�g�iTransform�j
    public Transform frontWheel;

    // �O�ւ̔��a�i�z�C�[���̉�]�ʌv�Z�Ɏg�p�j
    public float wheelRadius = 0.35f;

    // �O�ւ̉�]�p�x�iX����]�j
    private float frontWheelAngle = 10f;

   

    public void Rotate(float speed)
    {
        // �z�C�[���̉~�����v�Z�i2��r�j
        float circumference = 2f * Mathf.PI * wheelRadius;
        
        // ���t���[���Ői�񂾋������Z�o�i���x �~ ���ԁj
        float distance = speed * Time.deltaTime;
       
        // �ړ������Ɋ�Â��ăz�C�[������]����p�x���v�Z�i�x���j
        float deltaAngle = (distance / circumference) * 360f;

        // �z�C�[���̌��݂̉�]�p�ɉ��Z�i�ǂ�ǂ���j
        frontWheelAngle += deltaAngle;


        // �z�C�[����Transform�ɉ�]��K�p�iX����]�j
        if (frontWheel != null)
            frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);
    }


}
