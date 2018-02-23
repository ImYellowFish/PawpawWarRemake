using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sub component of the battleManager.
/// </summary>
public abstract class BattleComponent : MonoBehaviour {
    protected BattleManager battleManager;

    public virtual void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public abstract void CleanUp();
    public abstract void OnStartBattle();
    public abstract void OnEndBattle();
}
