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
            if (owner.Param.BoilTrigger.Peek())
            {
                owner.Timer.Start(StringConstants.TimerName.BoilTime,0.85f);
            }
            else if (owner.Param.JumpBackwardTrigger.Peek())
            {
                owner.Timer.Start(StringConstants.TimerName.JumpBackwardTime,0.8f);
            }

            _canTransition = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            Debug.Log("Player Boil State");
            
            if (owner.Timer.IsFinished(StringConstants.TimerName.BoilTime))
            {
                _canTransition = true;
            }
            
            if (owner.Timer.IsFinished(StringConstants.TimerName.JumpBackwardTime))
            {
                _canTransition = true; 
            }
            
        }
        
        public override void OnExit(PlayerMotion owner)
        {
            // owner.Param.BoilTrigger = false;
            // owner.Param.JumpBackward = false;
            owner.Timer.CleanupFinished();
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _canTransition);
            var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _canTransition);
            var attackAnim = new FSMCondition<PlayerMotion>(m => m.Param.AttackTrigger.Peek() && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
            AddCondition(attackAnim, StringConstants.AnimName.LightAttack1);
        }
    }
}

