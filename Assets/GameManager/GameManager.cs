using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameStart = false; // ゲーム開始フラグ

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始フラグを初期化
        isGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        isGameStart = true;
        // ゲーム開始時の処理をここに追加
        Debug.Log("ゲームが開始されました！");
    }

    public bool IsGameStarted()
    {
        return isGameStart;
    }
}
