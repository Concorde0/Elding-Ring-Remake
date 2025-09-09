using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    //TODO: 这些个bool值有朝一日会把他们优化的，而且目前这个延迟切换动画的问题很多，得抠细节
    //TODO: 这个_canBoil简直就是个屎山
    public class PlayerMoveState : FSMState<PlayerMotion>
    {
        private bool _canStop;
        private bool _canTransition;
        private bool _waitingForStop;
        private bool _canBoil;
        
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Timer.Start(StringConstants.TimerName.MoveToStop, 0.5f);
            _canStop = false;
            _canTransition = false;
            _waitingForStop = false;
            _canBoil = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            Debug.Log("Player Move State");
            
            if (owner.Param.IsSpecialHurt)
            {
                _canTransition = true;
                return;
            }
            
            if (!owner.Param.IsLocked)
            {
                owner.Motor.HandleInputRotation();
            }
            else
            {
                owner.Motor.HandleLockRotation();
            }
            
            owner.Motor.Move(owner.Param.MoveInput);
            
            if (owner.Param.BoilTrigger.Peek() || owner.Param.JumpBackwardTrigger.Peek())
            {
                _canTransition = true;
                _canBoil = true;
            }

            if (owner.Param.MoveInput.sqrMagnitude < 0.01f)
            {
               
                if (!_waitingForStop)
                {
                    _waitingForStop = true;
                    owner.Timer.Start(StringConstants.TimerName.MoveDelayTransition, 0.05f);
                }

                if (owner.Timer.IsFinished(StringConstants.TimerName.MoveDelayTransition))
                {
                    _canTransition = true;
                }
            }
            else
            {
                _waitingForStop = false;
            }

            if (owner.Timer.IsFinished(StringConstants.TimerName.MoveToStop))
            {
                _canStop = true;
            }
        }

        public override void OnExit(PlayerMotion owner)
        {
            base.OnExit(owner);
            owner.Timer.CleanupFinished();
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var hurtAnim = new FSMCondition<PlayerMotion>(m => m.Param.IsSpecialHurt && _canTransition);
            var runTurn = new FSMCondition<PlayerMotion>(m => m.Param.Run && m.Param.TurnTrigger.Peek() && _canStop);
            var stopAnim = new FSMCondition<PlayerMotion>(m =>  _canStop && _canTransition && !_canBoil);
            var idleAnim = new FSMCondition<PlayerMotion>(m =>  !_canStop && _canTransition && !_canBoil);
            var boilAnim = new FSMCondition<PlayerMotion>(m => _canBoil && _canTransition);
            var attackAnim = new FSMCondition<PlayerMotion>(m => m.Param.AttackTrigger.Peek());
            AddCondition(hurtAnim,StringConstants.AnimName.SpecialHurt);
            AddCondition(runTurn, StringConstants.AnimName.RunTurn);
            AddCondition(boilAnim, StringConstants.AnimName.BoilForward);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(stopAnim, StringConstants.AnimName.MoveStop);
            AddCondition(attackAnim, StringConstants.AnimName.LightAttack1);
        }
        
    }
}

