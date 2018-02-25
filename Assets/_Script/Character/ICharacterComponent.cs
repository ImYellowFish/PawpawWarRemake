using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

public interface ICharacterComponent
{
    void Init(Character ch);
    void CleanUp();
}

public abstract class CharacterComponent : SlaveComponent<Character>, ICharacterComponent
{
    public Character character
    {
        get
        {
            return host;
        }
    }
}