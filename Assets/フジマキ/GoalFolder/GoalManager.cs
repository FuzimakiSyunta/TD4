using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    // GoalScriptを取得するための参照
    private GoalScript goalScript;
    public GameObject goal;

    // リザルト画面のUIなど
    public GameObject result;
    public GameObject titleButton;

    // 前のゴール状態を保存
    private bool wasGoal = false;

    void Start()
    {
        // goalオブジェクトからGoalScriptを取得
        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }

        if (result != null)
        {
            result.SetActive(false); // 初期状態で非表示に
            titleButton.SetActive(false); // タイトルボタンも非表示に
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
