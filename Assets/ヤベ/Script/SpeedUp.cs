using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField]
    private JCKPlayerOperation jckPlayerOperation;
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
            jckPlayerOperation = other.GetComponent<JCKPlayerOperation>();
            jckPlayerOperation.Acceleration();
            Debug.Log("ダッシュボードに乗った");
        }
    }

}
