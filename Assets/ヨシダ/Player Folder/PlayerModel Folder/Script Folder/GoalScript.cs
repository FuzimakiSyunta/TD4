using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    //���݂̃��b�v��
    int lap = 0;
    //�K�v�ȃ��b�v��
    int needLap = 1;
    //�S�[���t���O
    bool goal = false;
    //�`�F�b�N�|�C���g�t���O
    private bool[] checkpoint = new bool[3] { false, false, false };
    // Start is called before the first frame update
    void Start()
    {
       goal = false;
       checkpoint = new bool[3] { false, false, false };
}

    // Update is called once per frame
    void Update()
    {
        if (lap == needLap)
        {
            goal = true;
            //�����ɃV�[���؂�ւ�������
            Debug.Log("�S�[������");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint1"))
        {
            checkpoint[0] = true;
            Debug.Log("�`�F�b�N�|�C���g1�ɐG�ꂽ");
        }

        if (collision.gameObject.CompareTag("Checkpoint2") && checkpoint[0] == true)
        {
            checkpoint[1] = true;
            Debug.Log("�`�F�b�N�|�C���g2�ɐG�ꂽ");
        }

        if (collision.gameObject.CompareTag("Checkpoint3") && checkpoint[0] == true&& checkpoint[1] ==true)
        {
            checkpoint[2] = true;
            Debug.Log("�`�F�b�N�|�C���g3�ɐG�ꂽ");
        }

        if (collision.gameObject.CompareTag("Goal") && checkpoint[0] == true && checkpoint[1] == true && checkpoint[2]==true)
        {
            checkpoint[0] = false;
            checkpoint[1] = false;
            checkpoint[2] = false;

            lap += 1;
            
            Debug.Log("�S�[��");

        }

    }

    public bool IsGoal()
    {
        return goal;
    }

}
