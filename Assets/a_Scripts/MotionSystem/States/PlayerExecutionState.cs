using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerExecutionState : FSMState<PlayerMotion>
{
    private bool _canTransition;
    public override void OnEnter(PlayerMotion owner)
    {
        owner.Stats.shouldPlayExecutionAnim = false;
        _canTransition = false;
        owner.Motor.Execution();
        owner.Timer.Start(StringConstants.TimerName.ExecutionTime,5.66f);
    }

    public override void OnUpdate(PlayerMotion owner)
    {
        if (owner.Timer.IsFinished(StringConstants.TimerName.ExecutionTime))
        {
            _canTransition = true;
        }
    }

    public override void OnExit(PlayerMotion owner)
    {
        
    }

    public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
    {
        var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _canTransition);
        var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _canTransition);
        AddCondition(idleAnim, StringConstants.AnimName.Idle);
        AddCondition(moveAnim, StringConstants.AnimName.Move);
    }
}
