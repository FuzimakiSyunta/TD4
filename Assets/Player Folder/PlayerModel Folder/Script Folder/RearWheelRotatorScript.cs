using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearWheelRotatorScript : MonoBehaviour
{
    // ��ւ�Transform�i��]������Ώۂ̃I�u�W�F�N�g�j
    public Transform rearWheel;
    
    // �z�C�[���̔��a�i��]�ʂ̌v�Z�Ɏg�p�j
    public float wheelRadius = 0.35f;
   
    // ��ւ̌��݂̉�]�p�iX���̊p�x��ێ��j
    private float rearWheelAngle = 0f;

    public void Rotate(float speed)
    {
        // �z�C�[���̉~���i2��r�j���v�Z
        float circumference = 2f * Mathf.PI * wheelRadius;
        
        // ���t���[���Ői�񂾋����i���x �~ �o�ߎ��ԁj
        float distance = speed * Time.deltaTime;
       
        // ���̋����ɑ��������]�p�x�i�x���j���Z�o
        float deltaAngle = (distance / circumference) * 360f;

        // ���݂̉�]�p�ɉ��Z�i�t���[�����Ƃɗݐρj
        rearWheelAngle += deltaAngle;

        // ��ւ�Transform��X����]��K�p�iZ���EY���͉񂳂Ȃ��j
        if (rearWheel != null)
            rearWheel.localRotation = Quaternion.Euler(rearWheelAngle, 0f, 0f);
    }
}
