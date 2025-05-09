using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadMarioKartScript : MonoBehaviour
{
    public float jumpForce = 20f; // ��ɒ��˂��
    public Vector3 launchDirection = new Vector3(0, 1, 1); // �O�{������Ȃǎ��R�ɐݒ�\

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerOperation>();
        if (player == null)
        {
            player = other.GetComponentInParent<PlayerOperation>(); // �q�I�u�W�F�N�g���G�ꂽ�ꍇ
        }

        if (player != null)
        {
            player.AddExternalForce(launchDirection.normalized * jumpForce);
        }
    }
}
