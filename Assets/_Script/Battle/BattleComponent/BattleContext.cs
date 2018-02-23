using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom battle settings when entering the battle
/// </summary>
[System.Serializable]
public class BattleContext{
    public int playerCount;
    public string levelName;
    public BattleLevel level;
}
