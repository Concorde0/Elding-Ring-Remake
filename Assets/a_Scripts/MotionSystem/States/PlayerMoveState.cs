using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
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
            owner.Timer.Start(StringConstants.AnimName.Move, 0.5f);
            _canStop = false;
            _canTransition = false;
            _waitingForStop = false;
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Move(owner.Param.moveInput);

            if (owner.Param.moveInput.sqrMagnitude < 0.01f)
            {
                if (!_waitingForStop)
                {
                    _waitingForStop = true;
                    owner.Timer.Start(StringConstants.AnimName.MoveStop, 0.03f);
                }
                
                if (owner.Timer.IsFinished(StringConstants.AnimName.MoveStop))
                {
                    _canTransition = true;
                }
            }
            
            else
            {
                _waitingForStop = false;
            }
            
            if (owner.Timer.IsFinished(StringConstants.AnimName.Move))
            {
                _canStop = true;
            }

            
            
            
        }
        
        //TODO: 目前，方向突变的时候，input会相互抵消，导致触发stop动画，也许需要状态缓存来解决许多问题
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var stopAnim = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.01f && _canStop && _canTransition);
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.01f && !_canStop && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(stopAnim, StringConstants.AnimName.MoveStop);
        }
        
    }
}

