using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconCheck : MonoBehaviour
{
    public bool isLeftJoyCon;  // true = ç∂, false = âE

    void Update()
    {
        float h, v;

        if (isLeftJoyCon)
        {
            h = Input.GetAxis("Horizontal 2_JoyCon");
            v = Input.GetAxis("Vertical 2_JoyCon");
        }
        else
        {
            h = Input.GetAxis("Horizontal 1_JoyCon");
            v = Input.GetAxis("Vertical 1_JoyCon");
        }

    }
}
