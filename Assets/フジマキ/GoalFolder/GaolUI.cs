using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaolUI : MonoBehaviour
{
    private GoalScript goalScript;
    public GameObject goal;
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        goalScript = GetComponent<GoalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goalScript.IsGoal()&& Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
