using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
{
    public Animator animator;

    private bool isGrounded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    
    void Update()
    {
        // ���E���
        animator.SetBool("FallLeft", Input.GetKey(KeyCode.A));
        animator.SetBool("FallRight", Input.GetKey(KeyCode.D));

        // �U��
        animator.SetBool("HitLeft", Input.GetKeyDown(KeyCode.E));
        animator.SetBool("HitRight", Input.GetKeyDown(KeyCode.Q));

        // �󒆂Ȃ�X�^���g�\
        if (!isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                animator.SetBool("SmallPose1", true);
            else
                animator.SetBool("SmallPose1", false);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                animator.SetBool("SmallPose2", true);
            else
                animator.SetBool("SmallPose2", false);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                animator.SetBool("SmallPose3", true);
            else
                animator.SetBool("SmallPose3", false);
        }
        else
        {
            // �n�ʂɂ���Ƃ��̓X�^���g�����ׂăI�t
            animator.SetBool("SmallPose1", false);
            animator.SetBool("SmallPose2", false);
            animator.SetBool("SmallPose3", false);
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

}