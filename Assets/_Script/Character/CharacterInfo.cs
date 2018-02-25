using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic info about character type, team, settings.
/// </summary>
[System.Serializable]
public class CharacterInfo{
    public int playerIndex;
    public string prefabName;
    public bool willCreateController;
    public int team;
    public Vector3 initialPos;

    public string prefabPath
    {
        get
        {
            // TODO: remove test
            return "Test/Character/" + prefabName;
        }
    }

    public static CharacterInfo GetDefault()
    {
        return new CharacterInfo
        {
            playerIndex = 0,
            prefabName = "Cube",
        };
    }
}
