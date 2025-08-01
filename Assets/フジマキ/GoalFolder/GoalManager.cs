using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    // GoalScriptを取得するための参照
    private GoalScript goalScript;
    public GameObject goal;
    // GameManagerの参照
    public GameManager gameManagerScript;
    public GameObject gameManager;

    // リザルト画面のUIなど
    public GameObject goalImage;
    public GameObject titleButton;

    // ミニマップ
    public GameObject Minimap;

    // 前のゴール状態を保存
    private bool wasGoal = false;

    void Start()
    {
        // goalオブジェクトからGoalScriptを取得
        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }

        // GameManagerの参照を取得
        if (gameManager != null)
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManagerが設定されていません。");
        }

        if (goalImage != null)
        {
            goalImage.SetActive(false); // 初期状態で非表示に
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
            if (goalImage != null)
            {
                goalImage.SetActive(isGoal);
                titleButton.SetActive(isGoal);
                Debug.Log("Result SetActive: " + isGoal);
            }

            wasGoal = isGoal;
        }

        if (goalScript.IsGoal()&&gameManagerScript.IsGameStarted())
        {
            // ゴール状態になったらミニマップを非表示にする
            if (Minimap != null)
            {
                Minimap.SetActive(false);
                Debug.Log("Minimap SetActive: false");
            }
        }
        else
        {
            // ゴール状態でない場合はミニマップを表示する
            if (Minimap != null)
            {
                Minimap.SetActive(true);
                Debug.Log("Minimap SetActive: true");
            }

        }
    }
}
