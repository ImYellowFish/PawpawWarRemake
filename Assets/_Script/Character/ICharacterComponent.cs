using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterComponent
{
    void Init(Character ch);
    void CleanUp();
}

public abstract class CharacterComponent : MonoBehaviour, ICharacterComponent
{
    protected Character character;

    public virtual void Init(Character ch)
    {
        this.character = ch;
    }

    public virtual void CleanUp()
    {

    }
}