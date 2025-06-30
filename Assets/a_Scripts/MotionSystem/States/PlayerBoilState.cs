using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerBoilState : FSMState<PlayerMotion>
    {
        private bool _canTransition;
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Motor.Boil();
            if (owner.Param.Boil)
            {
                owner.Timer.Start(StringConstants.TimerName.BoilTime,1f);
            }
            else if (owner.Param.JumpBackward)
            {
                owner.Timer.Start(StringConstants.TimerName.JumpBackwardTime,0.8f);
            }

            _canTransition = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            if (owner.Param.Boil)
            {
                if (owner.Timer.IsFinished(StringConstants.TimerName.BoilTime))
                {
                    _canTransition = true;
                }
            }
            else if (owner.Param.JumpBackward)
            {
                if (owner.Timer.IsFinished(StringConstants.TimerName.JumpBackwardTime))
                {
                    _canTransition = true;
                }
            }
            
        }
        
        public override void OnExit(PlayerMotion owner)
        {
            owner.Param.Boil = false;
            owner.Param.JumpBackward = false;
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _canTransition);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
        }
    }
}

