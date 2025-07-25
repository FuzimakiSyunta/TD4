using UnityEngine;
using TMPro;

public class DistanceToNearestCheckpoint : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public TextMeshProUGUI distanceText; // 距離表示用のテキスト
    public float reachThreshold = 0f; // 到達判定の距離閾値

    private Transform[] checkpoints; // チェックポイント配列
    private int currentIndex = 0; // 現在のチェックポイントインデックス

    void Start()
    {
        // チェックポイントの取得（名前で探す）
        checkpoints = new Transform[3];
        checkpoints[0] = GameObject.Find("CheckPoint1")?.transform;
        checkpoints[1] = GameObject.Find("CheckPoint2")?.transform;
        checkpoints[2] = GameObject.Find("CheckPoint3")?.transform;
    }

    void Update()
    {
        if (currentIndex >= checkpoints.Length)
        {
            distanceText.text = "ALL CHECKPOINTS CLEARED!";
            enabled = false;
            return;
        }

        if (player == null || distanceText == null || checkpoints[currentIndex] == null) return;

        Vector3 playerXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 checkpointXZ = new Vector3(checkpoints[currentIndex].position.x, 0, checkpoints[currentIndex].position.z);
        float distance = Vector3.Distance(playerXZ, checkpointXZ);

        distanceText.text = $"{distance:F0} m";

        if (distance <= reachThreshold)
        {
            currentIndex++;
        }
    }


}
