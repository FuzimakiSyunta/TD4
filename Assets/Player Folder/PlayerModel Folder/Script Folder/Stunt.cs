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
        // 例：スペースキーを押したらアニメーション再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            animator.SetBool("SmallPose3", true);
        }
        else 
        {
            animator.SetBool("SmallPose3", false);
        }

        // 例：スペースキーを押したらアニメーション再生
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