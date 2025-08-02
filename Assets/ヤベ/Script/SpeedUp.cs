using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
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
        if(other.tag=="PlayerAttack")
        {
            playerOperation = other.GetComponent<PlayerOperation>();
            playerOperation.Acceleration();
            Debug.Log("ダッシュボードに乗った");
        }
    }

}
