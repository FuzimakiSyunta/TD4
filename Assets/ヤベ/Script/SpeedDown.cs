using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HDROutputUtils;

public class SpeedDown : MonoBehaviour
{
    [SerializeField]
    private PlayerOperation playerOperation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    playerOperation.playerSpeed -= playerOperation.deceleration;
        //    Debug.Log("É_Å[ÉgÇ…èÊÇ¡ÇΩ");
        //}
    }
}
