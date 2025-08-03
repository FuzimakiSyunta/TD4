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
    // スピードメーター
    public GameObject SpeedMater;

    // ゴール時の画像の位置
    private Vector3 goalImageTargetPosition;
    private float slowSpeed = 600f;
    private float fastSpeed = 2000f;
    private float moveStartTime = 0f;
    private float slowDuration = 1f; // 1秒間はゆっくり

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
            goalImage.SetActive(false);
            titleButton.SetActive(false);

            // 初期位置 x = +1900
            goalImage.transform.localPosition = new Vector3(1900f, goalImage.transform.localPosition.y, goalImage.transform.localPosition.z);
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

                if (isGoal)
                {
                    goalImageTargetPosition = new Vector3(-1900f, goalImage.transform.localPosition.y, goalImage.transform.localPosition.z);
                    moveStartTime = Time.unscaledTime;
                    goalImage.SetActive(true);
                }
            }

            wasGoal = isGoal;
        }

        if (goalImage && goalImage != null)
        {
            float elapsed = Time.unscaledTime - moveStartTime;
            float speed = (elapsed < slowDuration) ? slowSpeed : fastSpeed;

            Vector3 current = goalImage.transform.localPosition;
            goalImage.transform.localPosition = Vector3.MoveTowards(
                current,
                goalImageTargetPosition,
                speed * Time.unscaledDeltaTime
            );

            if (Vector3.Distance(goalImage.transform.localPosition, goalImageTargetPosition) < 0.1f)
            {
                goalImage.SetActive(false);
            }
        }


        if (goalScript.IsGoal()&&gameManagerScript.IsGameStarted())
        {
            // ゴール状態になったらミニマップを非表示にする
            if (Minimap != null)
            {
                Minimap.SetActive(false);
                SpeedMater.SetActive(false); // スピードメーターも非表示に   
                Debug.Log("Minimap SetActive: false");
            }
        }
        else
        {
            // ゴール状態でない場合はミニマップを表示する
            if (Minimap != null)
            {
                Minimap.SetActive(true);
                SpeedMater.SetActive(true); // スピードメーターも表示する
                Debug.Log("Minimap SetActive: true");
            }

        }
    }
}
