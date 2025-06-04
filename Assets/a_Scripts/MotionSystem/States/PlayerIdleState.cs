using System.Collections;
using System.Collections.Generic;
using LitJson;
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

        public override void OnUpdate(PlayerMotion owner)
        {
        
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var moveInput = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1f);
            AddCondition(moveInput, StringConstants.AnimName.Move);
        }
    }
}

