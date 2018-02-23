using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

/// <summary>
/// Manages all the stats in battle, such as scores, records, etc.
/// </summary>
public class BattleStatistics : BattleComponent {
    // TODO: player related data and team related data
    [ReadOnly]
    public int[] scores;

    public void AddScore(int playerIndex, int score)
    {
        if(scores.Length <= playerIndex)
        {
            Debug.LogError("ScoreBoard length overflow: " + playerIndex + ", length: " + scores.Length);
            return;
        }
        scores[playerIndex] += score;
    }

    public void GetScore(int playerIndex, int score)
    {

    }

    public override void CleanUp()
    {
        scores = null;
    }

    public override void OnStartBattle()
    {
        scores = new int[4];   
    }

    public override void OnEndBattle()
    {
        battleManager.dispatcher.Dispatch(BattleManager.Event.ScoreSet, EmptyEventMessage.Instance);
    }
}
