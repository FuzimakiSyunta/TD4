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

    //次のシーン指定
    public string nextSceneName;

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
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    // スペースキーが押されたら次のシーンへ遷移
                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
                }
                Debug.Log("Result SetActive: " + isGoal);
            }

            wasGoal = isGoal;
        }
    }

}
