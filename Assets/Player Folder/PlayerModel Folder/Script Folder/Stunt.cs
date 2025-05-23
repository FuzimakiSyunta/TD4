using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ��F�X�y�[�X�L�[����������A�j���[�V�����Đ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            animator.SetBool("SmallPose3", true);
        }
        else 
        {
            animator.SetBool("SmallPose3", false);
        }

        // ��F�X�y�[�X�L�[����������A�j���[�V�����Đ�
        if (Input.GetKey(KeyCode.A))
        {

            animator.SetBool("FallLeft", true);
        }
        else
        {
            animator.SetBool("FallLeft", false);
        }

    }
}