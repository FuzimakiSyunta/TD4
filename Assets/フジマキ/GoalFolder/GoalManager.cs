using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    // GoalScript���擾���邽�߂̎Q��
    private GoalScript goalScript;
    public GameObject goal;

    // ���U���g��ʂ�UI�Ȃ�
    public GameObject Result;

    //���̃V�[���w��
    public string nextSceneName;

    // �O�̃S�[����Ԃ�ۑ�
    private bool wasGoal = false;

    void Start()
    {
        // goal�I�u�W�F�N�g����GoalScript���擾
        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }

        if (Result != null)
        {
            Result.SetActive(false); // ������ԂŔ�\����
        }
    }

    void Update()
    {
        if (goalScript == null) return;

        bool isGoal = goalScript.IsGoal();
        Debug.Log("IsGoal: " + isGoal);

        if (isGoal != wasGoal)
        {
            if (Result != null)
            {
                Result.SetActive(isGoal);
                Debug.Log("Result SetActive: " + isGoal);
            }

            wasGoal = isGoal;
        }
    }

}
