using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    //TODO: 这些个bool值有朝一日会把他们优化的，而且目前这个延迟切换动画的问题很多，得抠细节
    public class PlayerMoveState : FSMState<PlayerMotion>
    {
        private bool _canStop;
        private bool _canTransition;
        private bool _waitingForStop;
        public PlayerMoveState()
        {
            
        }
        public override void OnEnter(PlayerMotion owner)
        {
            owner.Timer.Start(StringConstants.TimerName.MoveToStop, 0.5f);
            _canStop = false;
            _canTransition = false;
            _waitingForStop = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Move(owner.Param.MoveInput);

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
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var stopAnim = new FSMCondition<PlayerMotion>(m =>  _canStop && _canTransition);
            var idleAnim = new FSMCondition<PlayerMotion>(m =>  !_canStop && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(stopAnim, StringConstants.AnimName.MoveStop);
        }
        
    }
}

