using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    private List<GoalScript> racers = new List<GoalScript>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterRacer(GoalScript racer)
    {
        if (!racers.Contains(racer))
        {
            racers.Add(racer);
        }
    }

    public void FinishRacer(GoalScript racer)
    {
        // ゴールしたらリストから外す（オプション）
        // racers.Remove(racer);
    }

    // ✅ ここで毎フレーム順位を更新
    public List<GoalScript> GetCurrentRanking()
    {
        racers.Sort((a, b) => b.GetProgress().CompareTo(a.GetProgress()));
        return racers;
    }

    public int GetCurrentRank(GoalScript racer)
    {
        var ranking = GetCurrentRanking();
        for (int i = 0; i < ranking.Count; i++)
        {
            if (ranking[i].GetInstanceID() == racer.GetInstanceID())
                return i + 1;
        }
        return -1;
    }
}
