using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerIdleState : FSMState<PlayerMotion>
    {
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Motor.Idle();
        }
    }
}

