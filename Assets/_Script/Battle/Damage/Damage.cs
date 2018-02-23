using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Records the damage info.
/// </summary>
public class Damage{
    public Character source;
    public Character target;
    public float damage;

    // TODO: damage type
    public Vector3 force;
}
