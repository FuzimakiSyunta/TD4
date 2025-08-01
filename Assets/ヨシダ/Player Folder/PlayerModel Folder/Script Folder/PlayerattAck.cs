using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class PlayerattAck : MonoBehaviour
{
    public GameObject Rhitbox; // 当たり判定のオブジェクト（初期状態は非アクティブ）
    public GameObject Lhitbox; // 当たり判定のオブジェクト（初期状態は非アクティブ）
    Stunt2 stunt2;

   

        // Start is called before the first frame update
    void Start()
    {
        stunt2 = GameObject.Find("Armature").GetComponent<Stunt2>();

    }

    // Update is called once per frame
    void Update()
    {
        if (stunt2.RightAttackAnimation() == true)
        {
            Rhitbox.SetActive(true);
           
        }
        else if(stunt2.RightAttackAnimation() == false)
        {
            Rhitbox.SetActive(false);
        }

        if (stunt2.AttackAnimation() == true)
        {
            Lhitbox.SetActive(true);
        }
        else 
        {
            Lhitbox.SetActive(false);
        }


    }
}
