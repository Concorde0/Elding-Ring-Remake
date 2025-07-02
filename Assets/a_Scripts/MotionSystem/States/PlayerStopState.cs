using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerStopState : FSMState<PlayerMotion>
    {
        private bool _canTransition;
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Timer.Start(StringConstants.AnimName.MoveStop,0.3f);
            _canTransition = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            Debug.Log("Player Stop State");
            owner.Motor.Stop();
            if (owner.Timer.IsFinished(StringConstants.AnimName.MoveStop))
            {
                _canTransition = true;
            }
        }
    
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => _canTransition && m.Param.MoveInput.sqrMagnitude < 0.05f);
            var moveAnim = new FSMCondition<PlayerMotion>(m => _canTransition && m.Param.MoveInput.sqrMagnitude >= 0.1);
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.Boil || m.Param.JumpBackward);
            var attackAnim = new FSMCondition<PlayerMotion>(m => m.Param.AttackTrigger.Peek());
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
            AddCondition(attackAnim, StringConstants.AnimName.LightAttack1);
        }
    
    }
}

