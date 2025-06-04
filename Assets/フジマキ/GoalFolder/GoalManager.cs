using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    // GoalScript���擾���邽�߂̎Q��
    private GoalScript goalScript;
    public GameObject goal;

    // ���U���g��ʂ�UI�Ȃ�
    public GameObject result;
    public GameObject titleButton;

    // �O�̃S�[����Ԃ�ۑ�
    private bool wasGoal = false;

    void Start()
    {
        // goal�I�u�W�F�N�g����GoalScript���擾
        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }

        if (result != null)
        {
            result.SetActive(false); // ������ԂŔ�\����
            titleButton.SetActive(false); // �^�C�g���{�^������\����
        }
    }

    void Update()
    {
        if (goalScript == null) return;

        bool isGoal = goalScript.IsGoal();
        Debug.Log("IsGoal: " + isGoal);

        if (isGoal != wasGoal)
        {
            if (result != null)
            {
                result.SetActive(isGoal);
                titleButton.SetActive(isGoal);
                
                Debug.Log("Result SetActive: " + isGoal);
            }

            wasGoal = isGoal;
        }
    }

}
