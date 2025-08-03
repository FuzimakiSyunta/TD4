using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HDROutputUtils;

public class SpeedDown : MonoBehaviour
{
    [SerializeField]
    private JCKPlayerOperation1 jckPlayerOperation1;
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
            jckPlayerOperation1 = other.GetComponent<JCKPlayerOperation1>();
            if (Input.GetKey(KeyCode.W)||JCScript.Instance.RightZRButton)
            {
                jckPlayerOperation1.playerSpeed = Mathf.MoveTowards(jckPlayerOperation1.playerSpeed, deceleration, 30f * Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.S) || JCScript.Instance.LeftZLButton)
            {
                jckPlayerOperation1.playerSpeed = Mathf.MoveTowards(jckPlayerOperation1.playerSpeed, -deceleration, 30f * Time.deltaTime);
            }
                Debug.Log("É_Å[ÉgÇ…èÊÇ¡ÇΩ");
        }
    }
}
