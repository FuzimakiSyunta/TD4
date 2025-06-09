using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMater : MonoBehaviour
{
    private PlayerOperation playerOperation;
    public GameObject playerOperationScript;

    private GameManager gameManager;
    public GameObject gamemanagerScript;

    public GameObject speedMater_BackImage;
    public GameObject tacoMeterImage;

    // 最小・最大角度と速度
    private float currentAngle = -120f;
    private const float minAngle = 90f;
    private const float maxAngle = -145f;
    private const float maxSpeed = 160f;

    // Start is called before the first frame update
    void Start()
    {
        playerOperation = playerOperationScript.GetComponent<PlayerOperation>(); // 修正: PlayerOperation コンポーネントを取得
        gameManager = gamemanagerScript.GetComponent<GameManager>(); // 修正: GameManager コンポーネントを取得
        //スピードメーターUI初期化
        speedMater_BackImage.SetActive(false);
        // タコメーター非表示
        tacoMeterImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SpeedMaterActive(); // スピードメーター/タコメーターの表示・非表示を更新
        TacoMeterMove(); // タコメーターの動きを更新
    }

    void SpeedMaterActive()
    {
        if (gameManager.IsGameStarted())
        {
            speedMater_BackImage.SetActive(true); // スピードメーター表示
            // タコメーターの表示
            tacoMeterImage.SetActive(true); // タコメーター表示
        }
        else
        {
            speedMater_BackImage.SetActive(false); // スピードメーター非表示
            tacoMeterImage.SetActive(false); // タコメーター非表示
        }
    }

    void TacoMeterMove()
    {
        float speed = playerOperation.GetPlayerSpeed();
        float normalized = Mathf.Clamp01(speed / maxSpeed);

        float targetAngle = Mathf.Lerp(minAngle, maxAngle, normalized);
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * 5f);

        tacoMeterImage.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
