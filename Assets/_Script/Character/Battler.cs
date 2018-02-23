using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages damage/skill
/// TODO: set faction when create character
/// </summary>
public class Battler : CharacterComponent
{
    /// <summary>
    /// Which team is the player in.
    /// 0 stands for neutral. 1 ~ 9 stands for player.
    /// </summary>
    public int team;

    public void TakeDamage(Damage damage)
    {

    }

    /// <summary>
    /// Is the character of the same team?
    /// </summary>
    public bool IsAlly(int team)
    {
        return this.team == team;
    }

    /// <summary>
    /// Is the character of the same team?
    /// </summary>
    public bool IsAlly(Character ch)
    {
        return this.team == ch.battler.team;
    }
}
