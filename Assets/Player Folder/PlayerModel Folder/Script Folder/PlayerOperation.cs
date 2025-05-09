using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
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
    float brakePower = 20f;

    // ��]���x(���E)
    float turnSpeed = 100f;
    
    // �X���i�o���N�j
    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;


    public Transform frontWheel;     // �O��
    public Transform rearWheel;      // ���
   
    public float wheelRadius = 0.35f;

    private float frontWheelAngle = 0f;
    private float rearWheelAngle = 0f;

    // �O������̃W�����v��
    private Vector3 externalVelocity = Vector3.zero;
  //  private Vector3 force;


    // Start is called before the first frame update
    void Start()
    {
        //������
        playerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        playerRotation = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[
        MovePlayer();

       

        RotateWheel();
    }

    //�v���C���[����
    void MovePlayer()
    {
        // ����������
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else
            playerSpeed -= deceleration * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            playerSpeed -= brakePower * Time.deltaTime;

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        // Y����]�i���FA�L�[�A�E�FD�L�[�j
        float turn = 0f;
        if (Input.GetKey(KeyCode.A))
            turn = -1f;
        else if (Input.GetKey(KeyCode.D))
            turn = 1f;

        // ��]�i���E�j - Y����]
        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        // Z���̃o���N�i�X���j���v�Z
        targetBank = Mathf.Lerp(targetBank, -turn * bankAngle, Time.deltaTime * bankLerpSpeed);

        // �o���N�i�X���j�𔽉f
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        // Z���̃o���N��K�p���ăv���C���[�̉�]��ݒ�
        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, currentBank);
        transform.rotation = targetRotation;

        // �O���ɐi��
        transform.position += transform.forward * playerSpeed * Time.deltaTime;

        // �O�����́i�W�����v�Ȃǁj��������
        transform.position += externalVelocity * Time.deltaTime;

        // �O���͂����X�Ɍ����i���R�ɗ�������悤�Ɂj
        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * 2f);

    }

    void RotateWheel()
    {
        // �^�C���̉~�� = 2��r
        float circumference = 2f * Mathf.PI * wheelRadius;

        // �i�񂾋��� = ���x �~ ����
        float distance = playerSpeed * Time.deltaTime;

        // ��]�p�i�x�j = �i�񂾋��� / ���� �~ 360��
        float deltaAngle = (distance / circumference) * 360f;

        // ��ցE�O�ւƂ��ɉ�]�iX���j
        rearWheelAngle += deltaAngle;
        frontWheelAngle += deltaAngle;

        rearWheel.localRotation = Quaternion.Euler(rearWheelAngle, 0f, 0f);
        frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);


    }

    internal void AddExternalForce(Vector3 force)
    {
        externalVelocity += force;
    }
}


