using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

/// <summary>
/// Sub component of the battleManager.
/// </summary>
public abstract class BattleComponent : SlaveComponent<BattleManager> {
    public BattleManager battleManager { get { return host; } }
    public abstract void OnStartBattle();
    public abstract void OnEndBattle();
}
