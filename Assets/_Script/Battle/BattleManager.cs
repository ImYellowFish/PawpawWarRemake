using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonsterLove.StateMachine;

public class BattleManager : IGameSystem {
    public enum State { Idle, Battle }
    public State currentState;
    private StateMachine<State> fsm;
    private BattleLevel level;

    public void StartBattle(BattleLevel level)
    {
        this.level = level;
        fsm.ChangeState(State.Battle);
        currentState = fsm.State;
    }

    public void StopBattle()
    {
        Destroy(level.gameObject);
        fsm.ChangeState(State.Idle);
        currentState = fsm.State;
    }


    private void OnIdleEnter()
    {

    }

    private void OnBattleEnter()
    {

    }
    
    public override void Init(GameManager manager)
    {
        fsm = StateMachine<State>.Initialize(this, State.Idle, false);
    }
    
    public override void CleanUp()
    {
        
    }

    public override void OnSceneLoaded(SceneInfo sceneInfo)
    {
        fsm.ChangeState(State.Idle);
    }
}
