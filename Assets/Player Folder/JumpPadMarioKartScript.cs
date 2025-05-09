using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadMarioKartScript : MonoBehaviour
{
    public float jumpForce = 20f; // 上に跳ねる力
    public Vector3 launchDirection = new Vector3(0, 1, 1); // 前＋上方向など自由に設定可能

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerOperation>();
        if (player == null)
        {
            player = other.GetComponentInParent<PlayerOperation>(); // 子オブジェクトが触れた場合
        }

        if (player != null)
        {
            player.AddExternalForce(launchDirection.normalized * jumpForce);
        }
    }
}
