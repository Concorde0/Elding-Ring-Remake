using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerHurtState : FSMState<PlayerMotion>
{
    private bool _canTransition;
    public override void OnEnter(PlayerMotion owner)
    {
        _canTransition = false;
        owner.Motor.Hurt();
        owner.Timer.Start(StringConstants.TimerName.SpecialHurtTime,3.26f);
    }

    public override void OnUpdate(PlayerMotion owner)
    {
        if (owner.Timer.IsFinished(StringConstants.TimerName.SpecialHurtTime))
        {
            _canTransition = true;
        }
    }

    public override void OnExit(PlayerMotion owner)
    {

        owner.Param.IsSpecialHurt = false;
    }

    public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
    {
        var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _canTransition);
        var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _canTransition);
        AddCondition(idleAnim, StringConstants.AnimName.Idle);
        AddCondition(moveAnim, StringConstants.AnimName.Move);
    }
    
}
