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
    // Start is called before the first frame update
    void Start()
    {
        goal = false;
        checkpoint = new bool[3] { false, false, false };
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
        if (lap == needLap)
        {
            goal = true;
            //ここにシーン切り替えを入れる
            //Debug.Log("ゴールした");
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
}
