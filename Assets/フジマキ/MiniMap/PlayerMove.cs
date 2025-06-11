using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    private float move;
    private float moveSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 時間依存の移動
        float move = moveSpeed * Time.deltaTime;
        //キーボード/////////////////////////////////
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(move, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-move, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, move);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -move);
        }
    }
}
