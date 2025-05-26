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
        // 左右回避
        animator.SetBool("FallLeft", Input.GetKey(KeyCode.A));
        animator.SetBool("FallRight", Input.GetKey(KeyCode.D));

        // 攻撃
        animator.SetBool("HitLeft", Input.GetKeyDown(KeyCode.E));
        animator.SetBool("HitRight", Input.GetKeyDown(KeyCode.Q));

        // 空中ならスタント可能
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
            // 地面にいるときはスタントをすべてオフ
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

}