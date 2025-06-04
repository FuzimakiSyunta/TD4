using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    //現在のラップ数
    int lap = 0;
    //必要なラップ数
    int needLap = 1;
    //ゴールフラグ
    bool goal = false;
    //チェックポイントフラグ
    private bool[] checkpoint = new bool[3] { false, false, false };
    // Start is called before the first frame update
    void Start()
    {
       goal = false;
       checkpoint = new bool[3] { false, false, false };
}

    // Update is called once per frame
    void Update()
    {
        if (lap == needLap)
        {
            goal = true;
            //ここにシーン切り替えを入れる
            Debug.Log("ゴールした");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint1"))
        {
            checkpoint[0] = true;
            Debug.Log("チェックポイント1に触れた");
        }

        if (collision.gameObject.CompareTag("Checkpoint2") && checkpoint[0] == true)
        {
            checkpoint[1] = true;
            Debug.Log("チェックポイント2に触れた");
        }

        if (collision.gameObject.CompareTag("Checkpoint3") && checkpoint[0] == true&& checkpoint[1] ==true)
        {
            checkpoint[2] = true;
            Debug.Log("チェックポイント3に触れた");
        }

        if (collision.gameObject.CompareTag("Goal") && checkpoint[0] == true && checkpoint[1] == true && checkpoint[2]==true)
        {
            checkpoint[0] = false;
            checkpoint[1] = false;
            checkpoint[2] = false;

            lap += 1;
            
            Debug.Log("ゴール");

        }

    }

    public bool IsGoal()
    {
        return goal;
    }

}
