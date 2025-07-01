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
            _isFinished = false;
            _canTransition = false;
            _comboIndex = 0;
            owner.Param.canAttack = false;
            owner.Motor.LightAttack(_comboIndex);
            owner.Timer.Start(StringConstants.TimerName.LightAttackTransitionTime1, 0.3f);
            owner.Timer.Start(StringConstants.TimerName.LightAttackTime1,1f);
        }

        
        //TODO:这是一坨屎，我现在手动记录time来控制clip何时可以转换，这是非常蠢的一件事，以后有精力的话我会回来优化的
        public override void OnUpdate(PlayerMotion owner)
        {
            Debug.Log("Player Attack State");
            if (_comboIndex == 0 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTransitionTime1))
            {
                owner.Param.canAttack = true;
                _canTransition = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTransitionTime1);
            }
            else if (_comboIndex == 1 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTransitionTime2))
            {
                owner.Param.canAttack = true;
                _canTransition = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTransitionTime2);
            }
            else if (_comboIndex == 2 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTransitionTime3))
            {
                _canTransition = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTransitionTime3);
            }
            
            
            
            if (_comboIndex == 0 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime1))
            {
                _isFinished = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTime1);
            }
            else if (_comboIndex == 1 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime2))
            {
                _isFinished = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTime2);
            }
            else if (_comboIndex == 2 && owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime3))
            {
                _isFinished = true;
                owner.Timer.Remove(StringConstants.TimerName.LightAttackTime3);
            }


            if (_canTransition && owner.Param.canAttack && owner.Param.TryConsumeAttack())
            {
                _comboIndex++;
                owner.Param.canAttack = false;
                _canTransition = false;

                if (_comboIndex == 1)
                {
                    owner.Motor.LightAttack(_comboIndex);
                    owner.Timer.Start(StringConstants.TimerName.LightAttackTransitionTime2, 0.3f);
                    owner.Timer.Start(StringConstants.TimerName.LightAttackTime2,1f);
                    owner.Timer.Remove(StringConstants.TimerName.LightAttackTime1);
                }
                else if (_comboIndex == 2)
                {
                    owner.Motor.LightAttack(_comboIndex);
                    owner.Timer.Start(StringConstants.TimerName.LightAttackTransitionTime3, 0.3f);
                    owner.Timer.Start(StringConstants.TimerName.LightAttackTime3,1f);
                    owner.Timer.Remove(StringConstants.TimerName.LightAttackTime2);
                }
                else
                {
                    // combo 超出最大数量
                    owner.Param.canAttack = false;
                    _comboIndex = 0;
                }
            }
        }

        public override void OnExit(PlayerMotion owner)
        {
            _comboIndex = 0;
            _isFinished = false;
            _canTransition = false;
            owner.Param.canAttack = true;
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

