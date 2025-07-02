using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerAttackState : FSMState<PlayerMotion>
    {
        private int _comboIndex;
        private bool _isFinished;
        private bool _canTransition;

        public override void OnEnter(PlayerMotion owner)
        {
            owner.Timer.Start(StringConstants.TimerName.LightAttackTime1,1f);
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            //何时可以进行下一阶段
            if (owner.Timer.IsElapsedInRange(StringConstants.TimerName.LightAttackTime1, 0.5f, 1f) && owner.Param.AttackTrigger.Consume())
            {
                _comboIndex++;
            }
        }
        public override void OnExit(PlayerMotion owner)
        {
            
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _isFinished);
            var moveAnim = new FSMCondition<PlayerMotion>(m =>  m.Param.MoveInput.sqrMagnitude >= 0.1 && _isFinished);
            var boilAnim = new FSMCondition<PlayerMotion>(m =>m.Param.Boil && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
        }
       
        
    }
}

