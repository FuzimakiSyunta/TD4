using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class JCStunt : MonoBehaviour
{
    public Animator animator;

    private bool isGrounded = false;

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

    PlayerActionState nextState = PlayerActionState.None;

    // Joy-Conスティックの入力閾値設定を追加
    [Header("Joy-Con Input Thresholds")]
    [Tooltip("スティックの傾きを検知する閾値（左右回避用）")]
    public float stickAvoidThreshold = 0.5f;
    [Tooltip("攻撃やスタントポーズ時のJoy-Con振り回し判定の加速度閾値")]
    //左右に突き出す際の加速度の閾値
    public float stuntSwingAccelThreshold = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        PlayerActionState currentPressedState = PlayerActionState.None;

        SetActionState(currentPressedState);

        if (!isGrounded)
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
            // 攻撃
            if (Input.GetKeyDown(KeyCode.E) || (JCScript.Instance != null && JCScript.Instance.IsRightSwinging && JCScript.Instance.RightAccel.x > stuntSwingAccelThreshold))
            {
                SetActionState(PlayerActionState.HitLeft);
            }
            if (Input.GetKeyDown(KeyCode.Q) || (JCScript.Instance != null && JCScript.Instance.IsLeftSwinging && JCScript.Instance.LeftAccel.x < -stuntSwingAccelThreshold))
            {
                SetActionState(PlayerActionState.HitRight);
            }
            //何も押してない
            if (currentPressedState != PlayerActionState.None)
            {
                SetActionState(PlayerActionState.None);
            }
        }
        // 空中のスタント可能
        if (!isGrounded)
            {
            if (Input.GetKeyDown(KeyCode.Alpha1) || (JCScript.Instance != null && JCScript.Instance.RightYButton && JCScript.Instance.IsRightSwinging))
            {
                SetActionState(PlayerActionState.SmallPose1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || (JCScript.Instance != null && JCScript.Instance.RightXButton && JCScript.Instance.IsRightSwinging))
            {
                SetActionState(PlayerActionState.SmallPose2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || (JCScript.Instance.IsRightSwinging)&& (JCScript.Instance.IsLeftSwinging))
            {
                SetActionState(PlayerActionState.SmallPose3);
            }
        }

    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("地面に接触している");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("地面から離れた");
        }
    }

    void SetActionState(PlayerActionState state)
    {
        if (nextState != state)
        {
            nextState = state;
            animator.SetInteger("ActionState", (int)nextState);
            Debug.Log($"State Changed to: {nextState}");
        }
    }


}