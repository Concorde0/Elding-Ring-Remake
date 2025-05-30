using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerStopState : FSMState<PlayerMotion>
{
    private float animTime = 0.15f;
    private bool animFinished;
    public override void OnEnter(PlayerMotion owner)
    {
        owner.Timer.Start(StringConstants.AnimName.MoveStop,animTime);
        animFinished = false;
    }

    public override void OnUpdate(PlayerMotion owner)
    {
        owner.Motor.Stop();
        if (owner.Timer.IsFinished(StringConstants.AnimName.MoveStop))
        {
            animFinished = true;
        }
    }
    
    public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
    {
        var idleAnim = new FSMCondition<PlayerMotion>(m => animFinished);
        AddCondition(idleAnim,StringConstants.AnimName.Idle);
    }
    
}
