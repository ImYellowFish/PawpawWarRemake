using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = BattleManager.State;
using MonsterLove.StateMachine;

/// <summary>
/// Controls the state of the battle.
/// Dispatches events when state changes.
/// </summary>
public class BattleStateMachine : BattleComponent {
    private StateMachine<State> fsm;
    public State CurrentState
    {
        get
        {
            return fsm.State;
        }
    }

    public State debug_current;

    public override void Init(BattleManager battleManager)
    {
        base.Init(battleManager);
        fsm = StateMachine<State>.Initialize(this, State.Idle, false);
    }

    public override void CleanUp()
    {
    }

    public override void OnStartBattle()
    {
        fsm.ChangeState(State.Battle);
    }

    public override void OnEndBattle()
    {
        fsm.ChangeState(State.Idle);
    }

    private void Update()
    {
        if(fsm != null)
        {
            debug_current = fsm.State;
        }
    }
}
