using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunt2 : MonoBehaviour
{
    public Animator animator;
    GroundContactDetection groundContactDetection;
    public int currentScore = 0;
    private HashSet<PlayerActionState> scoredStates = new HashSet<PlayerActionState>();
    private float previousNormalizedTime = 0f;

    // Joy-Conスティックの入力閾値設定を追加
    [Header("Joy-Con Input Thresholds")]
    [Tooltip("スティックの傾きを検知する閾値（左右回避用）")]
    public float stickAvoidThreshold = 0.5f;
    [Tooltip("攻撃やスタントポーズ時のJoy-Con振り回し判定の加速度閾値")]
    //左右に突き出す際の加速度の閾値
    public float stuntSwingAccelThreshold = 0.5f;

    private enum PlayerActionState
    {
        None = 0,
        FallLeft = 1,
        FallRight = 2,
        HitLeft = 3,
        HitRight = 4,
        SmallPose1 = 5,
        SmallPose2 = 6,
        SmallPose3 = 7
    }

    private Dictionary<PlayerActionState, int> actionScores = new Dictionary<PlayerActionState, int>
　　{
        //スコアを加算設定
    　　{ PlayerActionState.SmallPose1, 50 },
    　　{ PlayerActionState.SmallPose2, 75 },
    　　{ PlayerActionState.SmallPose3, 100 }
　　};

    PlayerActionState nextState = PlayerActionState.None;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        groundContactDetection = GameObject.Find("Player").GetComponent<GroundContactDetection>();
    }


    void Update()
    {
        PlayerActionState currentPressedState = PlayerActionState.None;

        SetActionState(currentPressedState);


        if (groundContactDetection.isGrounded == true)
        {
            // 左右回避
            if (Input.GetKey(KeyCode.A) || (JCScript.Instance != null && JCScript.Instance.LeftStick.x < -stickAvoidThreshold))
            {
                SetActionState(PlayerActionState.FallLeft);
            }
            if (Input.GetKey(KeyCode.D) || (JCScript.Instance != null && JCScript.Instance.LeftStick.x > stickAvoidThreshold))
            {
                SetActionState(PlayerActionState.FallRight);
            }

            //何も押してない
            if (currentPressedState != PlayerActionState.None)
            {
                SetActionState(PlayerActionState.None);
            }
        }
        if (groundContactDetection.isGrounded == false)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1) || (JCScript.Instance != null && JCScript.Instance.RightYButton && JCScript.Instance.IsRightSwinging))
            {
                SetActionState(PlayerActionState.SmallPose1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || (JCScript.Instance != null && JCScript.Instance.RightXButton && JCScript.Instance.IsRightSwinging))
            {
                SetActionState(PlayerActionState.SmallPose2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || (JCScript.Instance.IsRightSwinging) && (JCScript.Instance.IsLeftSwinging))
            {
                SetActionState(PlayerActionState.SmallPose3);
            }
        }


        if (Input.GetKeyDown(KeyCode.Q) || (JCScript.Instance != null && JCScript.Instance.IsLeftSwinging && JCScript.Instance.LeftAccel.x < -stuntSwingAccelThreshold))
        {
            SetActionState(PlayerActionState.HitRight);
            RightAttackAnimation();

        }
        if (Input.GetKeyDown(KeyCode.E) || (JCScript.Instance != null && JCScript.Instance.IsRightSwinging && JCScript.Instance.RightAccel.x > stuntSwingAccelThreshold))
        {
            SetActionState(PlayerActionState.HitLeft);
            LeftAttackAnimation();

        }




    }

    public bool IsSmallPoseAnimating()
    {
        // アニメーション中かどうか調べる
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        bool isPlaying =
            (info.IsName("SmallPose1") || info.IsName("SmallPose2") || info.IsName("SmallPose3")) &&
            info.normalizedTime < 1f;

        return isPlaying;
    }

    public void  CheckAndAddScore()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // SmallPose1～3のいずれか
        PlayerActionState? currentState = null;
        if (info.IsName("SmallPose1")) currentState = PlayerActionState.SmallPose1;
        else if (info.IsName("SmallPose2")) currentState = PlayerActionState.SmallPose2;
        else if (info.IsName("SmallPose3")) currentState = PlayerActionState.SmallPose3;

        if (currentState.HasValue)
        {
            float currentTime = info.normalizedTime;

            // normalizedTimeが 1 → 0 にループした（≒アニメーション1周完了）とき
            if (Mathf.Floor(previousNormalizedTime) < Mathf.Floor(currentTime))
            {
                currentScore += actionScores[currentState.Value];
                Debug.Log($"スコア加算: +{actionScores[currentState.Value]} (合計: {currentScore})");
            }

            previousNormalizedTime = currentTime;
        }
        else
        {
            // ポーズ外ならリセット
            previousNormalizedTime = 0f;
        }
       
     

    }



    public bool RightAttackAnimation()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName("HitRight") && info.normalizedTime < 1f;
    }

    public bool LeftAttackAnimation()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName("HitLeft") && info.normalizedTime < 1f;
    }
    void SetActionState(PlayerActionState state)
        {
            if (nextState != state)
            {
                nextState = state;
                animator.SetInteger("ActionState", (int)nextState);
                //Debug.Log($"State Changed to: {nextState}");
            }
        }
    
}
