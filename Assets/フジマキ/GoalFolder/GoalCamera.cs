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

    //次のシーン指定
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
            // ゴール状態が切り替わった時だけ実行
            if (isGoal)
            {
                // ゴール時：ゲームカメラOFF、ゴールカメラON
                gameCamera.enabled = false;
                goalCamera.enabled = true;
            }
            else
            {
                // ゴール前：ゲームカメラON、ゴールカメラOFF
                gameCamera.enabled = true;
                goalCamera.enabled = false;
            }

            wasGoal = isGoal;
        }

        if(goalCamera.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // スペースキーが押されたら次のシーンへ遷移
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
        }

    }
}
