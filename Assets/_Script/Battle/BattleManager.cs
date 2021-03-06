﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImYellowFish.Utility;

// TODO: onsceneloaded handling
public class BattleManager : IGameSystem
{
    /// <summary>
    /// The state of current Battle
    /// </summary>
    public enum State { Idle, Battle }
    public State CurrentState
    {
        get { return stateMachine.CurrentState; }
    }

    /// <summary>
    /// current level gameObject. 
    /// The level script will be used to control the battle rules, gameflow, etc.
    /// </summary>
    [ReadOnly]
    public BattleLevel level;

    /// <summary>
    /// Cusomized battle settings, such as playerCount, difficulty, etc.
    /// </summary>
    [ReadOnly]
    public BattleContext context;

    #region Battle components
    // Sub components
    public SlaveComponentContainer<BattleManager, BattleComponent> slaveContainer = new SlaveComponentContainer<BattleManager, BattleComponent>();

    /// <summary>
    /// The stateMachine for the battle.
    /// </summary>
    public BattleStateMachine stateMachine;

    /// <summary>
    /// Manages all the characters in this battle.
    /// </summary>
    public CharacterManager characters;

    /// <summary>
    /// Score board of the battle.
    /// TODO: change to stat manager.
    /// </summary>
    public BattleStatistics stat;
    #endregion

    #region Event system
    public enum Event
    {
        BattleStart = 0,
        BattleEnd = 1,

        ScoreSet = 10,

        AddCharacter = 20,
        RemoveCharacter = 21,
    }
    public Dispatcher<Event> dispatcher = new Dispatcher<Event>();
    #endregion

    #region Operations
    /// <summary>
    /// Start the battle by custom level and context
    /// </summary>
    public void StartBattle(BattleLevel level, BattleContext context)
    {
        this.level = level;
        this.context = context;

        foreach (var sc in slaveContainer.Slaves)
        {
            sc.OnStartBattle();
        }

    }

    /// <summary>
    /// End the current battle.
    /// </summary>
    public void EndBattle()
    {
        foreach (var sc in slaveContainer.Slaves)
        {
            sc.OnEndBattle();
        }
        Destroy(level.gameObject);
    }
    #endregion

    #region Lifecycle
    public override void Init(GameManager manager)
    {
        base.Init(manager);
        stateMachine = CreateSubComponent<BattleStateMachine>();
        characters = CreateSubComponent<CharacterManager>();
        stat = CreateSubComponent<BattleStatistics>();

    }

    public override void CleanUp()
    {
        foreach (var sc in slaveContainer.Slaves)
        {
            sc.CleanUp();
        }
    }
    
    /// <summary>
    /// Create a battle component by type
    /// </summary>
    private T CreateSubComponent<T>() where T : BattleComponent
    {
        return slaveContainer.CreateSlaveComponent<T>(this);
    }
    #endregion
}
