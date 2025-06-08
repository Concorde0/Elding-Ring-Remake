using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerStopState : FSMState<PlayerMotion>
{
    private bool _canTransition;
    public override void OnEnter(PlayerMotion owner)
    {
        owner.Timer.Start(StringConstants.AnimName.MoveStop,0.3f);
        _canTransition = false;
    }

    public override void OnUpdate(PlayerMotion owner)
    {
        owner.Motor.Stop();
        if (owner.Timer.IsFinished(StringConstants.AnimName.MoveStop))
        {
            _canTransition = true;
        }
    }
    
    public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
    {
        var idleAnim = new FSMCondition<PlayerMotion>(m => _canTransition && m.Param.MoveInput.sqrMagnitude < 0.05f);
        AddCondition(idleAnim, StringConstants.AnimName.Idle);
        var moveAnim = new FSMCondition<PlayerMotion>(m => _canTransition && m.Param.MoveInput.sqrMagnitude >= 0.1);
        AddCondition(moveAnim, StringConstants.AnimName.Move);
        var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.Boil || m.Param.JumpBackward);
        AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
    }
    
}
