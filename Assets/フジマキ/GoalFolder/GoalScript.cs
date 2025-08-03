using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript: MonoBehaviour
{
    //現在のラップ数
    int lap = 0;
    //必要なラップ数
    int needLap = 1;
    //ゴールフラグ
    bool goal = false;
    //チェックポイントフラグ
    private bool[] checkpoint = new bool[3] { false, false, false };
    //チェックポイントUI
    public GameObject[] checkPointUI; // 順番に消すオブジェクト

    public TMPro.TextMeshProUGUI rankText;
    // Start is called before the first frame update
    void Start()
    {
        goal = false;
        checkpoint = new bool[3] { false, false, false };

        // ✅ これが必須！登録していないと順位に含まれない
        RaceManager.Instance.RegisterRacer(this);

        if (checkPointUI.Length >= 3)
        {
            checkPointUI[0].SetActive(true);
            checkPointUI[1].SetActive(false);
            checkPointUI[2].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(goal) return;

        int rank = RaceManager.Instance.GetCurrentRank(this);

        // ログ（順位確認用）
        Debug.Log(name + " の現在順位: " + rank + "位");

        // 表示用
        if (rankText != null)
        {
            rankText.text = RankToString(rank);
        }

        // ゴール判定
        if (lap == needLap)
        {
            goal = true;
            RaceManager.Instance.FinishRacer(this);
        }


    }

    string RankToString(int rank)
    {
        switch (rank)
        {
            case 1: return "1";
            case 2: return "2";
            case 3: return "3";
            case 4: return "4";
            case 5: return "5";
            default: return rank + "位";
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint1"))
        {
            checkpoint[0] = true;
            if (checkPointUI.Length >= 2)
            {
                checkPointUI[0].SetActive(false);
                checkPointUI[1].SetActive(true);
            }
            Debug.Log("チェックポイント1に触れた");
        }

        if (collision.gameObject.CompareTag("Checkpoint2") && checkpoint[0] == true)
        {
            checkpoint[1] = true;
            if (checkPointUI.Length >= 3)
            {
                checkPointUI[1].SetActive(false);
                checkPointUI[2].SetActive(true);
            }
            Debug.Log("チェックポイント2に触れた");
        }

        if (collision.gameObject.CompareTag("Checkpoint3") && checkpoint[0] && checkpoint[1])
        {
            checkpoint[2] = true;
            if (checkPointUI.Length >= 3)
            {
                checkPointUI[2].SetActive(false);
            }
            Debug.Log("チェックポイント3に触れた");
        }

        if (collision.gameObject.CompareTag("Goal") && checkpoint[0] && checkpoint[1] && checkpoint[2])
        {
            checkpoint[0] = checkpoint[1] = checkpoint[2] = false;
            lap += 1;
            Debug.Log("ゴール");
        }
    }

    public bool IsGoal()
    {
        return goal;
    }

    public int GetProgress()
    {
        int progress = lap * 1000;

        for (int i = 0; i < checkpoint.Length; i++)
        {
            if (checkpoint[i]) progress += 100;
            else break;
        }

        // より精密に進捗をつける（前進距離などを加味）
        progress += (int)transform.position.z;

        return progress;
    }
}
