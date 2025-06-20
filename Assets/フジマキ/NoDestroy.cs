using UnityEngine;

public class NoDestroy : MonoBehaviour
{
    private void Awake()
    {
        // �������O�̃I�u�W�F�N�g�����łɑ��݂��Ă���Δj���i�d���h�~�j
        if (GameObject.Find(gameObject.name) != this.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
