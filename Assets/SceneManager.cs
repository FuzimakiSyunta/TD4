using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string targetSceneName = "GameScene"; // ˆÚ“®æ‚ÌƒV[ƒ“

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name != targetSceneName)
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}
