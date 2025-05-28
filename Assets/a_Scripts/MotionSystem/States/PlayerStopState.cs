using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerStopState : FSMState<PlayerMotion>
{
    public override void OnEnter(PlayerMotion owner)
    {
        base.OnEnter(owner);
    }

    public override void OnUpdate(PlayerMotion owner)
    {
        base.OnUpdate(owner);
    }
}
