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
            // ゴール状態が切り替わった時だけ実行
            if (isGoal)
            {
                // ゴール時：ゲームカメラOFF、ゴールカメラON
                gameCamera.enabled = false;
                goalCame.enabled = true;
            }
            else
            {
                // ゴール前：ゲームカメラON、ゴールカメラOFF
                gameCamera.enabled = true;
                goalCame.enabled = false;
            }

            wasGoal = isGoal;
        }
    }
}
