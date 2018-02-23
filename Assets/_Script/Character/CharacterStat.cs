using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : CharacterComponent
{
    public float maxHp;
    public float moveAcceleration;
    public float jumpAcceleration;
    public float maxHorizontalSpeed;

    // current value
    public float currentHp;

    public override void Init(Character ch)
    {
        base.Init(ch);
        currentHp = maxHp;
    }
}
