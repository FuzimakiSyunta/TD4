using UnityEngine;
using TMPro;

public class DistanceToNearestCheckpoint : MonoBehaviour
{
    public TextMeshProUGUI distanceText;

    [HideInInspector]
    public Transform[] checkpoints;

    [HideInInspector]
    public int currentIndex = 0;

    void Start()
    {
        checkpoints = new Transform[3];
        checkpoints[0] = GameObject.Find("CheckPoint1")?.transform;
        checkpoints[1] = GameObject.Find("CheckPoint2")?.transform;
        checkpoints[2] = GameObject.Find("CheckPoint3")?.transform;
    }

    void Update()
    {
        if (distanceText == null || currentIndex >= checkpoints.Length || checkpoints[currentIndex] == null) return;

        float distance = Vector3.Distance(transform.position, checkpoints[currentIndex].position);
        distanceText.text = $"CHECKPOINT {currentIndex + 1} : {distance:F1} m";
    }

    public void AdvanceCheckpoint()
    {
        currentIndex++;
        if (currentIndex >= checkpoints.Length)
        {
            distanceText.text = "ALLCHECKPOINT！";
            enabled = false; // 表示更新を止める
        }
    }
}
