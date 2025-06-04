using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
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
            // ���E���
            if (Input.GetKey(KeyCode.A))
            {
                SetActionState(PlayerActionState.FallLeft);
            }
            if (Input.GetKey(KeyCode.D))
            {
                SetActionState(PlayerActionState.FallRight);
            }
            // �U��
            if (Input.GetKeyDown(KeyCode.E))
            {
                SetActionState(PlayerActionState.HitLeft);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetActionState(PlayerActionState.HitRight);
            }
            //���������ĂȂ�
            if (currentPressedState != PlayerActionState.None)
            {
                SetActionState(PlayerActionState.None);
            }
        }
        // �󒆂̃X�^���g�\
        if (!isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetActionState(PlayerActionState.SmallPose1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActionState(PlayerActionState.SmallPose2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
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
            Debug.Log("�n�ʂɐڐG���Ă���");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("�n�ʂ��痣�ꂽ");
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