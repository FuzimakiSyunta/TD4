using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundContactDetection : MonoBehaviour
{
    public bool isGrounded;
    int totalScore = 0;
    Stunt2 stunt2;
    PlayerOperation playerOperation;
    public GameObject missObject;
    public GameObject parfectObject;


    // Start is called before the first frame update
    void Start()
    {
        stunt2 = GameObject.Find("Armature").GetComponent<Stunt2>();
        playerOperation = GameObject.Find("Player").GetComponent<PlayerOperation>();
        missObject.SetActive(false);
        parfectObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded == false)
        {
            stunt2.CheckAndAddScore();
        }
        //着地失敗
        if (stunt2.IsSmallPoseAnimating() && isGrounded == true)
        {
            playerOperation.playerSpeed = 0;
            stunt2.currentScore = 0;
            StartCoroutine(ShowTemporaryObject(missObject, 2f));
        }
        else if(isGrounded ==true)
        {
            //成功
            if (stunt2.currentScore > 0)
            {
                totalScore += stunt2.currentScore;
                playerOperation.Acceleration();
                //オブジェクトを出現一定時間で消える
                StartCoroutine(ShowTemporaryObject(parfectObject, 2f));
            }

            stunt2.currentScore = 0;
        }

       

    }

    void OnCollisionStay(Collision collision)
    {
        bool grounded = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject obj = contact.otherCollider.gameObject;
            if (obj.CompareTag("Ground") || obj.CompareTag("Savable"))
            {
                grounded = true;
                break;
            }
        }

        isGrounded = grounded;

        if (isGrounded)
        {
            
           // Debug.Log("地面に接触している");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Savable"))
        {
            isGrounded = false;
           
            //Debug.Log("地面から離れた");
        }
    }



    public bool GetisGrounbed()
    {
        return isGrounded;
    }


    IEnumerator ShowTemporaryObject(GameObject obj, float duration)
    {
        obj.SetActive(true);                  // 表示
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);                 // 消す
    }
}
