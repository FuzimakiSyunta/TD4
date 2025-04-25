using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    //�v���C���[��Position
    Vector3  playerPosition = Vector3.zero;
   
    //�v���C���[�̉�]
    Vector3 playerRotation = Vector3.zero;

    //�v���C���[�X�s�[�h
    float playerSpeed = 0;
    
    //����
    float acceleration = 10f;

    // �����x
    float deceleration = 10f;

    float maxSpeed = 100f;

    //�u���[�L�̌���
    float brakePower = 20f;



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
    }

    //�v���C���[����
    void MovePlayer()
    {
        //�ړ�����
        if (Input.GetKey(KeyCode.W))
        {
            //����
            playerSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            // �����i�L�[�𗣂����玩�R�Ɍ����j
            playerSpeed -= deceleration * Time.deltaTime;
        }

        //�u���[�L�̏���
        if (Input.GetKey(KeyCode.S))
        {
            // �u���[�L�i���������j
            playerSpeed -= brakePower * Time.deltaTime;
        }

        //�X�s�[�h�𐧌�
        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        //�O�����ɐi�ޏ���
        playerPosition.z += playerSpeed * Time.deltaTime;

        //���E�Ɉړ����鏈��
        if(Input.GetKey(KeyCode.D)) 
        {
            playerPosition.x +=  playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerPosition.x -= playerSpeed * Time.deltaTime;
        }

        //�ʒu�̔��f
        transform.position = playerPosition;
    }
}
