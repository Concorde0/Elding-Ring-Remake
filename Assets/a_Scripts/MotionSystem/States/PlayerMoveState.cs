using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerMoveState : FSMState<PlayerMotion>
    {
        private bool _canStop;
        public PlayerMoveState()
        {
            
        }
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Timer.Start(StringConstants.AnimName.Move, 0.5f);
            _canStop = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Move(owner.Param.moveInput);

            if (owner.Timer.IsFinished(StringConstants.AnimName.Move))
            {
                _canStop = true;
            }
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var stopAnim = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.05f && _canStop);
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.05f && !_canStop);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(stopAnim, StringConstants.AnimName.MoveStop);
        }
        
    }
}

