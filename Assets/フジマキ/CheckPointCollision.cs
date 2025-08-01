using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCollision : MonoBehaviour
{
    public GameObject[] targets; // 順番に消すオブジェクト
    private int currentIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentIndex < targets.Length)
        {
            targets[currentIndex].SetActive(false);
            currentIndex++;
        }
    }

}
