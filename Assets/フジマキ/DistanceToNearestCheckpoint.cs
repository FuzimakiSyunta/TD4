using UnityEngine;
using TMPro;

public class DistanceToNearestCheckpoint : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI[] distanceText;
    public Transform[] checkpoints;
    public float reachThreshold = 0f;
    private int currentIndex = 0;
    public GoalScript goalScript;
    public GameObject goal;

    void Start()
    {
        if (goal != null)
        {
            goalScript = goal.GetComponent<GoalScript>();
        }
    }

    void Update()
    {
        if (player == null || distanceText == null || checkpoints == null || goalScript == null) return;

        // ゴールしたら距離表示をクリアして停止
        if (goalScript.IsGoal())
        {
            foreach (var text in distanceText)
            {
                if (text != null) text.text = "";
            }
            enabled = false;
            return;
        }

        // 距離を計算して表示
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

        // 一つのチェックポイントに近づいたら次へ
        if (currentIndex < checkpoints.Length)
        {
            Vector3 targetCheckpointXZ = new Vector3(checkpoints[currentIndex].position.x, 0, checkpoints[currentIndex].position.z);
            float currentDistance = Vector3.Distance(playerXZ, targetCheckpointXZ);
            if (currentDistance <= reachThreshold)
            {
                currentIndex++;

                // 最後まで行ったら次のラップへ繰り返す（ゴールしない限り）
                if (currentIndex >= checkpoints.Length)
                {
                    currentIndex = 0;
                }
            }
        }
    }
}
