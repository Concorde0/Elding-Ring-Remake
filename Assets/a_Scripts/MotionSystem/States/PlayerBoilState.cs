using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerBoilState : FSMState<PlayerMotion>
    {
       
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Motor.Boil();
            
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            
        }
        
        public override void OnExit(PlayerMotion owner)
        {
            base.OnExit(owner);
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m =>m.Param.MoveInput.sqrMagnitude < 0.05f);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
        }
    }
}

