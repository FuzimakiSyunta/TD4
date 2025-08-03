using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HDROutputUtils;

public class SpeedDown : MonoBehaviour
{
    [SerializeField]
    private JCKPlayerOperation jckPlayerOperation;
    public float deceleration;
    
    // Start is called before the first frame update
    void Start()
    {
        deceleration = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            jckPlayerOperation = other.GetComponent<JCKPlayerOperation>();
            if (Input.GetKey(KeyCode.W)||JCScript.Instance.RightZRButton)
            {
                jckPlayerOperation.playerSpeed = Mathf.MoveTowards(jckPlayerOperation.playerSpeed, deceleration, 30f * Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.S) || JCScript.Instance.LeftZLButton)
            {
                jckPlayerOperation.playerSpeed = Mathf.MoveTowards(jckPlayerOperation.playerSpeed, -deceleration, 30f * Time.deltaTime);
            }
                Debug.Log("É_Å[ÉgÇ…èÊÇ¡ÇΩ");
        }
    }
}
