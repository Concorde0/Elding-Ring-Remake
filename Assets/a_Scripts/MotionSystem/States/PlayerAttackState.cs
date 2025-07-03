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
        private int _lastTriggeredComboIndex = -1;

        public override void OnEnter(PlayerMotion owner)
        {
            _comboIndex = 0;
            _isFinished = false;
            _canTransition = false;
            _lastTriggeredComboIndex = -1;
            owner.Timer.Start(StringConstants.TimerName.LightAttackTime1,2.3f);
            owner.Motor.LightAttack(_comboIndex);
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            Debug.Log("Attack State");
            //何时点击可以进行下一阶段
            if (_comboIndex == 0 && _lastTriggeredComboIndex < 0 && owner.Timer.IsElapsedInRange(StringConstants.TimerName.LightAttackTime1, 0.83f, 1f) && owner.Param.AttackTrigger.Consume())
            {
                Debug.Log("into Attack2");
                _comboIndex++;
                _lastTriggeredComboIndex = 0;
                _canTransition = false;
                owner.Motor.LightAttack(_comboIndex);
                owner.Timer.Start(StringConstants.TimerName.LightAttackTime2,2.2f);
            }
            else if (_comboIndex == 1 && _lastTriggeredComboIndex < 1 && owner.Timer.IsElapsedInRange(StringConstants.TimerName.LightAttackTime2, 0.9f, 1.16f) && owner.Param.AttackTrigger.Consume())
            {
                Debug.Log("into Attack3");
                _comboIndex++;
                _lastTriggeredComboIndex = 1;
                _canTransition = false;
                owner.Motor.LightAttack(_comboIndex);
                owner.Timer.Start(StringConstants.TimerName.LightAttackTime3,2.5f);
            }
            
            //如果没有再次输出，Attack播放到动画结束
            if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime1) && _comboIndex == 0)
            {
                Debug.Log("Attack1 finished");
                _isFinished = true;
            }
            else if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime2) && _comboIndex == 1)
            {
                Debug.Log("Attack2 finished");
                _isFinished = true;
            }
            else if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime3) && _comboIndex == 2)
            {
                Debug.Log("Attack3 finished");
                _isFinished = true;
            }
            
            //何时可以打断攻击（这里应该和连击时间的最后时间段一样？这个我还得进游戏确认一下）
            if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime1) >=  1.16f && _comboIndex == 0)
            {
                Debug.Log("Attack1 interrupted");
                _canTransition = true;
            }
            else if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime2) >= 1.16f && _comboIndex == 1)
            {
                Debug.Log("Attack2 interrupted");
                _canTransition = true;
            }
            else if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime3) >= 1.6f && _comboIndex == 2)
            {
                Debug.Log("Attack3 interrupted");
                _canTransition = true;
            }
            
        }
        public override void OnExit(PlayerMotion owner)
        {
            
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _isFinished);
            var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _isFinished);
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.BoilTrigger.Peek() && _canTransition);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
        }
       
        
    }
}

