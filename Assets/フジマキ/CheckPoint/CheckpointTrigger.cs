using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex; // このチェックポイントが何番目か指定

    private void OnTriggerEnter(Collider other)
    {
        DistanceToNearestCheckpoint tracker = other.GetComponent<DistanceToNearestCheckpoint>();
        if (tracker != null && tracker.currentIndex == checkpointIndex)
        {
            tracker.AdvanceCheckpoint();
        }
    }
}
