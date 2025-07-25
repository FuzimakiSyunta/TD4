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
    // Start is called before the first frame update
    void Start()
    {
        stunt2 = GameObject.Find("Armature").GetComponent<Stunt2>();
        playerOperation = GameObject.Find("Player").GetComponent<PlayerOperation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded == false)
        {
            stunt2.CheckAndAddScore();
        }
        //íÖíné∏îs
        if (stunt2.IsSmallPoseAnimating() && isGrounded == true)
        {
            playerOperation.playerSpeed = 0;
            stunt2.currentScore = 0;
        }
        else if(isGrounded ==true)
        {
            if (stunt2.currentScore > 0)
            {
                totalScore += stunt2.currentScore;
                playerOperation.Acceleration();
                Debug.Log($"totalScore: {totalScore}");
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
            
           // Debug.Log("ínñ Ç…ê⁄êGÇµÇƒÇ¢ÇÈ");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Savable"))
        {
            isGrounded = false;
           
            //Debug.Log("ínñ Ç©ÇÁó£ÇÍÇΩ");
        }
    }



    public bool GetisGrounbed()
    {
        return isGrounded;
    }
}
