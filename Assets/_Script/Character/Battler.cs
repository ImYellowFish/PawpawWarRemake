using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : CharacterComponent
{
    /// <summary>
    /// Which team is the player in.
    /// 0 stands for neutral. 1 ~ 9 stands for player.
    /// </summary>
    public int faction;

    public void TakeDamage(Damage damage)
    {

    }

    public bool IsFaction(int faction)
    {
        return this.faction == faction;
    }
}
