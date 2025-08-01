using UnityEngine;
using TMPro;

public class DistanceToNearestCheckpoint : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI[] distanceText;
    public Transform[] checkpoints; // ← 追加：チェックポイントを手動設定
    public float reachThreshold = 0f;
    private int currentIndex = 0;

    void Update()
    {
        if (player == null || distanceText == null || checkpoints == null) return;

        Vector3 playerXZ = new Vector3(player.position.x, 0, player.position.z);

        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] != null && distanceText[i] != null)
            {
                Vector3 checkpointXZ = new Vector3(checkpoints[i].position.x, 0, checkpoints[i].position.z);
                float distance = Vector3.Distance(playerXZ, checkpointXZ);
                distanceText[i].text = $"{distance:F0} m";
            }
        }

        if (currentIndex < checkpoints.Length)
        {
            Vector3 targetCheckpointXZ = new Vector3(checkpoints[currentIndex].position.x, 0, checkpoints[currentIndex].position.z);
            float currentDistance = Vector3.Distance(playerXZ, targetCheckpointXZ);
            if (currentDistance <= reachThreshold)
            {
                currentIndex++;
            }

            if (currentIndex >= checkpoints.Length)
            {
                enabled = false;
            }
        }
    }
}

