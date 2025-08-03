using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField]
    private JCKPlayerOperation1 jckPlayerOperation1;
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
            jckPlayerOperation1 = other.GetComponent<JCKPlayerOperation1>();
            jckPlayerOperation1.Acceleration();
            Debug.Log("ダッシュボードに乗った");
        }
    }

}
