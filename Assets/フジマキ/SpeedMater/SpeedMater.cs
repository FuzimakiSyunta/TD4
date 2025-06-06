using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMater : MonoBehaviour
{
    private PlayerMove playerMoveScript;
    public GameObject playerMove;

    private GameManager gameManager;
    public GameObject gamemanagerScript;

    public GameObject speedMaterImage;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMoveScript = GetComponent<PlayerMove>();
        gameManager = GetComponent<GameManager>();
        //スピードメーターUI初期化
        speedMaterImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.IsGameStarted())
        {
            speedMaterImage.SetActive(true);//スピードメーター表示
            
        }else 
        {
            speedMaterImage.SetActive(false);//スピードメーター非表示
        }
    }
}
