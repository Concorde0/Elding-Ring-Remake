using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerMoveState : FSMState<PlayerMotion>
    {
        public PlayerMoveState()
        {
            
        }
        public override void OnEnter(PlayerMotion owner)
        {
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Move(owner.Param.moveInput);
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.05f);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
        }
        
    }
}

