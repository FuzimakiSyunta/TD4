using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCamera : MonoBehaviour
{
    private GoalScript goalScript;
    public GameObject goal;

    public Camera gameCamera;
    public Camera goalCame;

    private bool wasGoal = false;

    private void Start()
    {
        gameCamera.enabled = true;
        goalCame.enabled = false;

        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }
    }

    private void Update()
    {
        if (goalScript == null) return;

        bool isGoal = goalScript.IsGoal();

        if (isGoal != wasGoal)
        {
            // �S�[����Ԃ��؂�ւ�������������s
            if (isGoal)
            {
                // �S�[�����F�Q�[���J����OFF�A�S�[���J����ON
                gameCamera.enabled = false;
                goalCame.enabled = true;
            }
            else
            {
                // �S�[���O�F�Q�[���J����ON�A�S�[���J����OFF
                gameCamera.enabled = true;
                goalCame.enabled = false;
            }

            wasGoal = isGoal;
        }
    }
}
