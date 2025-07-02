using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JCScript : MonoBehaviour
{
    //どこからでもアクセスできるようにする
    public static JCScript Instance { get; private set; }

    private List<Joycon> joycons;
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    ///<summary>
    ///左Joy-Conのスティック入力
    ///</summary>
    public Vector2 LeftStick { get; private set; }

    ///<summary>
    ///右Joy-Conのスティック入力
    ///</summary>
    public Vector2 RightStick { get; private set; }

    ///<summary>
    ///左Joy-ConのLボタン
    ///</summary>
    public bool LeftLButton { get; private set; }
    ///<summary>
    ///左Joy-ConのZLボタン
    ///</summary>
    public bool LeftZLButton { get; private set; }
    ///<summary>
    ///右Joy-ConのRボタン
    ///</summary>
    public bool RightRButton { get; private set; }
    ///<summary>
    ///右Joy-ConのZRボタン
    ///</summary>
    public bool RightZRButton { get; private set; }

    ///<summary>
    ///右Joy-ConのAボタン
    ///</summary>
    public bool RightAButton { get; private set; }
    ///<summary>
    ///右Joy-ConのBボタン
    ///</summary>
    public bool RightBButton { get; private set; }
    ///<summary>
    ///右Joy-ConのXボタン
    ///</summary>
    public bool RightXButton { get; private set; }
    ///<summary>
    ///右Joy-ConのYボタン
    ///</summary>
    public bool RightYButton { get; private set; }

    ///<summary>
    ///左Joy-Conの十字キー上ボタン
    ///</summary>
    public bool LeftDPadUp { get; private set; }
    ///<summary>
    ///左Joy-Conの十字キー下ボタン
    ///</summary>
    public bool LeftDPadDown { get; private set; }
    ///<summary>
    ///左Joy-Conの十字キー左ボタン
    ///</summary>
    public bool LeftDPadLeft { get; private set; }
    ///<summary>
    ///左Joy-Conの十字キー右ボタン
    ///</summary>
    public bool LeftDPadRight { get; private set; }

    ///<summary>
    ///左Joy-Conの加速度センサー
    ///</summary>
    public Vector3 LeftAccel { get; private set; }
    ///<summary>
    ///右Joy-Conの加速度センサー
    ///</summary>
    public Vector3 RightAccel { get; private set; }

    //加速度センサーによる振り回し検出
    [Header("Joy-Con Motion Detection Settings")]
    [Tooltip("加速度センサーの絶対値の合計がこの値を超えると振り回しと判定")]
    public float swingThreshold = 5f;
    [Tooltip("振り回し状態をリセットするまでの時間")]
    public float swingResetTime = 0.5f;

    private bool isLeftSwinging = false;
    private bool isRightSwinging = false;
    private float leftSwingTimer = 0f;
    private float rightSwingTimer = 0f;

    ///<summary>
    ///左Joy-Conが振り回されているか(短時間での大きな加速度変化)
    ///</summary>
    public bool IsLeftSwinging => isLeftSwinging;

    ///<summary>
    ///右Joy-Conが振り回されているか(短時間での大きな加速度変化)
    ///</summary>
    public bool IsRightSwinging => isRightSwinging;

    [Tooltip("「突き出し」と判定する加速度の最低値 (どの方向でも)")]
    public float thrustThreshold = 3.0f; // この値は調整が必要
    [Tooltip("「突き出し」状態が持続する最短時間")]
    public float thrustDuration = 0.1f; // 突き出しの瞬間的な動きを検知する持続時間

    private bool isLeftThrusting = false;
    private float leftThrustTimer = 0f;
    private bool isRightThrusting = false;
    private float rightThrustTimer = 0f;

    //各Joy-Conが最後に突き出しを検知した時刻
    public float LastLeftThrustTime { get; private set; } = -Mathf.Infinity;
    public float LastRightThrustTime { get; private set; } = -Mathf.Infinity;

    ///<summary>
    ///左Joy-Conが「突き出された」か (短時間での大きな加速度変化、方向不問)
    ///</summary>
    public bool IsLeftThrusting => isLeftThrusting;

    ///<summary>
    ///右Joy-Conが「突き出された」か (短時間での大きな加速度変化、方向不問)
    ///</summary>
    public bool IsRightThrusting => isRightThrusting;

    void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
    }
    else
    {
        Instance = this;
        //シーンをまたいでもこのマネージャーが破棄されないようにする
        DontDestroyOnLoad(gameObject);
    }
}

void Start()
{
    InitializeJoycons();
}

void Update()
{
    UpdateJoyconInputs();
    UpdateSwingDetection();
}

private void InitializeJoycons()
{
    //JoyconManager.Instanceが存在するか確認
    if (JoyconManager.Instance != null)
    {
        joycons = JoyconManager.Instance.j;
        Debug.Log($"Found {joycons.Count} Joy-Cons.");

        //左右のJoy-Conを識別
        leftJoycon = joycons.FirstOrDefault(jc => jc.isLeft);
        rightJoycon = joycons.FirstOrDefault(jc => !jc.isLeft);

        if (leftJoycon == null) Debug.LogWarning("左Joy-Conが見つかりません。左Joy-Conの操作は無効になります。");
        if (rightJoycon == null) Debug.LogWarning("右Joy-Conが見つかりません。右Joy-Conの操作は無効になります。");
    }
    else
    {
        Debug.LogError("JoyconManager.Instance が見つかりません。Joy-Con操作は無効になります。プロジェクトにJoyconManagerプレハブを配置しているか確認してください。");
        joycons = new List<Joycon>();
    }
}

private void UpdateJoyconInputs()
{
    //各Joy-Conからの入力を取得し、プロパティに格納
    if (leftJoycon != null)
    {
        float[] stickL = leftJoycon.GetStick();
        LeftStick = new Vector2(stickL[0], stickL[1]);

        LeftLButton = leftJoycon.GetButton(Joycon.Button.SHOULDER_1);
        LeftZLButton = leftJoycon.GetButton(Joycon.Button.SHOULDER_2);

        LeftDPadUp = leftJoycon.GetButton(Joycon.Button.DPAD_UP);
        LeftDPadDown = leftJoycon.GetButton(Joycon.Button.DPAD_DOWN);
        LeftDPadLeft = leftJoycon.GetButton(Joycon.Button.DPAD_LEFT);
        LeftDPadRight = leftJoycon.GetButton(Joycon.Button.DPAD_RIGHT);

        LeftAccel = leftJoycon.GetAccel();
    }
    else
    {
        LeftStick = Vector2.zero;
        LeftLButton = false;
        LeftZLButton = false;
        LeftDPadUp = false;
        LeftDPadDown = false;
        LeftDPadLeft = false;
        LeftDPadRight = false;
        LeftAccel = Vector3.zero;
    }

    if (rightJoycon != null)
    {
        float[] stickR = rightJoycon.GetStick();
        RightStick = new Vector2(stickR[0], stickR[1]);

        RightRButton = rightJoycon.GetButton(Joycon.Button.SHOULDER_1);
        RightZRButton = rightJoycon.GetButton(Joycon.Button.SHOULDER_2);

        RightAButton = rightJoycon.GetButton(Joycon.Button.DPAD_RIGHT);
        RightBButton = rightJoycon.GetButton(Joycon.Button.DPAD_DOWN); 
        RightXButton = rightJoycon.GetButton(Joycon.Button.DPAD_UP);   
        RightYButton = rightJoycon.GetButton(Joycon.Button.DPAD_LEFT); 

        RightAccel = rightJoycon.GetAccel();
    }
    else
    {
        RightStick = Vector2.zero;
        RightRButton = false;
        RightZRButton = false;
        RightAButton = false;
        RightBButton = false;
        RightXButton = false;
        RightYButton = false;
        RightAccel = Vector3.zero;
    }
}

private void UpdateSwingDetection()
{
    //左Joy-Conの振り回し検出
    if (leftJoycon != null)
    {
        //加速度の絶対値の合計が大きい場合を振り回しと判定
        if (LeftAccel.magnitude > swingThreshold)
        {
            isLeftSwinging = true;
            leftSwingTimer = swingResetTime;
        }

        if (isLeftSwinging)
        {
            leftSwingTimer -= Time.deltaTime;
            if (leftSwingTimer <= 0)
            {
                isLeftSwinging = false;
            }
        }
    }
    else
    {
        isLeftSwinging = false;
        leftSwingTimer = 0f;
    }


    //右Joy-Conの振り回し検出
    if (rightJoycon != null)
    {
        if (RightAccel.magnitude > swingThreshold)
        {
            isRightSwinging = true;
            rightSwingTimer = swingResetTime;
        }

        if (isRightSwinging)
        {
            rightSwingTimer -= Time.deltaTime;
            if (rightSwingTimer <= 0)
            {
                isRightSwinging = false;
            }
        }
    }
    else
    {
        isRightSwinging = false;
        rightSwingTimer = 0f;
    }
}
    private void UpdateThrustDetection()
    {
        //左Joy-Conの突き出し検出
        if (leftJoycon != null)
        {
            //加速度のいずれかの軸の絶対値が閾値を超えたら突き出しと判定
            //どの方向でも良いようにx,y,z
            if (Mathf.Abs(LeftAccel.x) > thrustThreshold ||
                Mathf.Abs(LeftAccel.y) > thrustThreshold ||
                Mathf.Abs(LeftAccel.z) > thrustThreshold)
            {
                //ついた時間
                if (!isLeftThrusting)
                {
                    LastLeftThrustTime = Time.time;
                }
                isLeftThrusting = true;
                //持続時間設定
                leftThrustTimer = thrustDuration; 
            }

            if (isLeftThrusting)
            {
                leftThrustTimer -= Time.deltaTime;
                if (leftThrustTimer <= 0)
                {
                    isLeftThrusting = false;
                }
            }
        }
        else
        {
            isLeftThrusting = false;
            //未接続なら時刻をリセット
            LastLeftThrustTime = -Mathf.Infinity; 
        }

        //右Joy-Conの突き出し検出
        if (rightJoycon != null)
        {
            if (Mathf.Abs(RightAccel.x) > thrustThreshold ||
                Mathf.Abs(RightAccel.y) > thrustThreshold ||
                Mathf.Abs(RightAccel.z) > thrustThreshold)
            {
                //ついた時間
                if (!isRightThrusting)
                {
                    LastRightThrustTime = Time.time;
                }
                isRightThrusting = true;
                rightThrustTimer = thrustDuration;
            }

            if (isRightThrusting)
            {
                rightThrustTimer -= Time.deltaTime;
                if (rightThrustTimer <= 0)
                {
                    isRightThrusting = false;
                }
            }
        }
        else
        {
            isRightThrusting = false;
            //同時じゃなければ初期化
            LastRightThrustTime = -Mathf.Infinity; 
        }
    }
}