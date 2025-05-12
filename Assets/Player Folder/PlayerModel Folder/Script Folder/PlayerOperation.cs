using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    //�v���C���[��Position
    Vector3 playerPosition = Vector3.zero;

    //�v���C���[�̉�]
    Vector3 playerRotation = Vector3.zero;

    //�X�s�[�h
    float playerSpeed = 0f;
    //����
    float acceleration = 100f;
    //����
    float deceleration = 50f;
    
    //�ō����x
    float maxSpeed = 500f;
    
    //�u���[�L�ɂ�錸��
    float brakePower = 50f;

    // ��]���x(���E)
    float turnSpeed = 100f;
    
    // �X���i�o���N�j
    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

    




    [SerializeField] LayerMask groundLayer; // �n�ʃ��C���[���w��


    // Start is called before the first frame update
    void Start()
    {
        //������
        playerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        playerRotation = new Vector3(30.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[
        MovePlayer();      

        StickToGround();

        //�^�C���̉�]����
        if (frontWheelRotator != null)
        {
            frontWheelRotator.Rotate(playerSpeed);
        }
        if (rearWheelRotator != null)
        {
            rearWheelRotator.Rotate(playerSpeed);
        }

    }

  

    //�v���C���[����
    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            playerSpeed -= deceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerSpeed -= brakePower * Time.deltaTime;
        }

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        targetBank = Mathf.Lerp(targetBank, -turn * bankAngle, Time.deltaTime * bankLerpSpeed);
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, currentBank);
        transform.rotation = targetRotation;

        transform.position += transform.forward * playerSpeed * Time.deltaTime;
       
    }

   

    

   

    // �n�ʂɋz�������鏈��
    void StickToGround()
    {
        if (!IsGrounded()) return;

        Ray ray = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, groundLayer))
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

            // �n�ʂ̌X���ɍ��킹�ĉ�]�i�I�v�V�����j
            Quaternion groundRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, groundRotation, Time.deltaTime * 5f);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }
}


