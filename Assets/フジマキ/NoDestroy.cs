using UnityEngine;

public class NoDestroy : MonoBehaviour
{
    private void Awake()
    {
        // 同じ名前のオブジェクトがすでに存在していれば破棄（重複防止）
        if (GameObject.Find(gameObject.name) != this.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
