using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCamera : MonoBehaviour
{
    private GoalScript goalScript;
    public GameObject goal;

    public Camera gameCamera;
    public Camera goalCamera;

    private bool wasGoal = false;

    //���̃V�[���w��
    public string nextSceneName;

    private void Start()
    {
        gameCamera.enabled = true;
        goalCamera.enabled = false;

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
                goalCamera.enabled = true;
            }
            else
            {
                // �S�[���O�F�Q�[���J����ON�A�S�[���J����OFF
                gameCamera.enabled = true;
                goalCamera.enabled = false;
            }

            wasGoal = isGoal;
        }

        if(goalCamera.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // �X�y�[�X�L�[�������ꂽ�玟�̃V�[���֑J��
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
        }

    }
}
