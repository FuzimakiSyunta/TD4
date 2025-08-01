using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUIFade : MonoBehaviour
{
    // GoalScript‚ğæ“¾‚·‚é‚½‚ß‚ÌQÆ
    private GoalScript goalScript;
    public GameObject goal;
    private GameManager gameManagerScript;
    public GameObject gameManager;

    public GameObject Minimap;
    // Start is called before the first frame update
    void Start()
    {
        goalScript= goal.GetComponent<GoalScript>();
        gameManagerScript= gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MinimapFade();
    }

    void MinimapFade()
    {
        if(goalScript.IsGoal())
        {
            Minimap.SetActive(false);
        }else if(gameManagerScript.IsGameStarted())
        {
            Minimap.SetActive(true);
        }
    }
}
