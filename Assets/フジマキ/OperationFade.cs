using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationFade : MonoBehaviour
{
    private bool isoperation = false;
    public GameObject operationUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnUI();

    }

    void OnUI()
    {
       
        if (Input.GetKeyDown(KeyCode.Q)/*||JCScript.Instance.RightYButton*/)
        {
            // èàóùì‡óe
            isoperation = true;
            operationUI.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.B)/*|| JCScript.Instance.RightBButton*/)
        {
            isoperation = false;
            operationUI.SetActive(false);
        }
    }
    public bool IsOperation()
    {
        return isoperation;
    }
}
