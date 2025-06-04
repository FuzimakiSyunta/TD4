using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSurface : MonoBehaviour
{
    public Transform frontWheel;
    public float wheelRadius = 0.35f;
    public LayerMask groundLayer;   // �ǉ�: �n�ʃ��C���[��ݒ�ł���悤��

    private float frontWheelAngle = 10f;
    public float groundCheckDistance = 0.5f; // Ray�̋���
    // ��̊p�x�i�n�ʂ̖@������̌X���j
    public float slopeAngle { get; private set; }

    private Vector3 groundNormal = Vector3.up;

    private void Start()
    {
        UpdateSlopeAngle();
    }

    // �n�ʂ̖@�����X�V���ČX�����v�Z����i�蓮�Ăяo���\�j
    public void UpdateSlopeAngle()
    {
        Collider col = GetComponent<Collider>();

        // �R���C�_�[�̌�����`��ɂ��@���̎����͏󋵂ɉ����ĕς���K�v����
        // �����ł͒P���ɏ�����Ɖ���
        groundNormal = transform.up;
        slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
    }

    // ���x�␳�p�̔{���ȂǕԂ����\�b�h���p�Ӊ\
    public float GetSpeedMultiplier()
    {
        // ��: �X�����傫���قǌ�������i�₪�}�Ȃ瑬�x�����ɂ���C���[�W�j
        return Mathf.Clamp01(1f - (slopeAngle / 45f));
    }

    public void Rotate(float speed)
    {
        // �^�C���̉�]����
        float circumference = 2f * Mathf.PI * wheelRadius;
        float distance = speed * Time.deltaTime;
        float deltaAngle = (distance / circumference) * 360f;

        frontWheelAngle += deltaAngle;

        if (frontWheel != null)
            frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);

        // �ڒn����iRaycast�j
        if (frontWheel != null)
        {
            Ray ray = new Ray(frontWheel.position, Vector3.down);
            bool isGrounded = Physics.Raycast(ray, groundCheckDistance, groundLayer);

            // ��F�ڒn���Ă���΃��O�o�́i�K�v�ɉ����ď�����ǉ����Ă��������j
            if (isGrounded)
            {
                Debug.Log("Front wheel is on the ground.");
            }
            else
            {
                Debug.Log("Front wheel is NOT on the ground.");
            }
        }
    }
}
