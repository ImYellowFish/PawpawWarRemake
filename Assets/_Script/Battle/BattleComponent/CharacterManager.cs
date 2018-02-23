using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

/// <summary>
/// Manages all the characters in the battle scene;
/// Add or remove a character.
/// </summary>
public class CharacterManager : BattleComponent {
    // settings
    // TODO: read from battle user context
    public int playerCount = 4;

    /// <summary>
    /// The list of all characters in the current scene
    /// </summary>
    [ReadOnly]
    public List<Character> all;
    
    /// <summary>
    /// Initialize the characterManager
    /// </summary>
    /// <param name="battleManager"></param>
    public override void Init(BattleManager battleManager)
    {
        base.Init(battleManager);
        all = new List<Character>();
    }

    public override void CleanUp()
    {
    }

    public override void OnStartBattle()
    {
        //playerCount = battleManager.context.playerCount;

        for(int i = 0; i < playerCount; i++)
        {
            var info = CharacterInfo.GetDefault();
            info.willCreateController = i == 0;
            info.playerIndex = i;
            CreateCharacter(info);
        }
    }

    public override void OnEndBattle()
    {
        foreach(var ch in all)
        {
            ch.CleanUp();
            Destroy(ch.gameObject);
        }
        all.Clear();
    }

    /// <summary>
    /// Creates a new character.
    /// TODO: add/remove during the battle
    /// TODO: put this in a special utility class
    /// </summary>
    public Character CreateCharacter(CharacterInfo info)
    {
        var prefab = Resources.Load<GameObject>(info.prefabPath);
        if(prefab == null)
        {
            Debug.LogError("Cannot find character prefab: " + info.prefabPath);
            return null;
        }

        var obj = Instantiate(prefab);
        var character = obj.GetComponent<Character>();
        character.Init(info, battleManager);
        all.Add(character);

        battleManager.dispatcher.Dispatch(BattleManager.Event.AddCharacter, new FixedEventMessage(character));

        return character;
    }
}
