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
        
        private int _expectedFrameDuration;

        public override void OnEnter(PlayerMotion owner)
        {
            _comboIndex = 0;
            _isFinished = false;
            _canTransition = false;
            _lastTriggeredComboIndex = -1;
            owner.Timer.Start(StringConstants.TimerName.LightAttackTime1,2.3f);
            _expectedFrameDuration = Mathf.CeilToInt(owner.Anim.GetAnimLength(StringConstants.TimerName.LightAttackTimeFrame1));
            owner.Timer.Start(StringConstants.TimerName.LightAttackTimeFrame1, _expectedFrameDuration);
            owner.Motor.LightAttack(_comboIndex);
            if (owner.Param.IsLion)
            {
                owner.Motor.ResetLionSmash();
                owner.Motor.LionSmash();
                owner.Timer.Start(StringConstants.TimerName.LionSmashTime, 4f);
            }
            
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            if (owner.Param.IsSpecialHurt)
            {
                _canTransition = true;
                return;
            }

            if (owner.Param.IsLion && owner.Timer.IsFinished(StringConstants.TimerName.LionSmashTime))
            {
                owner.Param.IsLion = false;
                _canTransition = true;
            }
            //何时点击可以进行下一阶段
            if (_comboIndex == 0 && _lastTriggeredComboIndex < 0 && owner.Timer.IsElapsedInRange(StringConstants.TimerName.LightAttackTime1, 0.95f, 1.4f) && owner.Param.AttackTrigger.Consume())
            {
                GameLoop.Instance.Hitbox.SetActive(false);
                
                _comboIndex++;
                _lastTriggeredComboIndex = 0;
                _canTransition = false;
                owner.Motor.LightAttack(_comboIndex);
                owner.Timer.Start(StringConstants.TimerName.LightAttackTime2,2.2f);
            }
            else if (_comboIndex == 1 && _lastTriggeredComboIndex < 1 && owner.Timer.IsElapsedInRange(StringConstants.TimerName.LightAttackTime2, 1f, 1.3f) && owner.Param.AttackTrigger.Consume())
            {
                
                GameLoop.Instance.Hitbox.SetActive(false);
                
                _comboIndex++;
                _lastTriggeredComboIndex = 1;
                _canTransition = false;
                owner.Motor.LightAttack(_comboIndex);
                owner.Timer.Start(StringConstants.TimerName.LightAttackTime3,2.5f);
            }
            
            //如果没有再次输出，Attack播放到动画结束
            if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime1) && _comboIndex == 0)
            {
                _isFinished = true;
            }
            else if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime2) && _comboIndex == 1)
            {
                _isFinished = true;
            }
            else if (owner.Timer.IsFinished(StringConstants.TimerName.LightAttackTime3) && _comboIndex == 2)
            {
                _isFinished = true;
            }
            
            //何时可以打断攻击（这里应该和连击时间的最后时间段一样？这个我还得进游戏确认一下）
            if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime1) >=  1.3f && _comboIndex == 0)
            {
                _canTransition = true;
            }
            else if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime2) >= 1.3f && _comboIndex == 1)
            {
                _canTransition = true;
            }
            else if (owner.Timer.GetElapsed(StringConstants.TimerName.LightAttackTime3) >= 1.8f && _comboIndex == 2)
            {
                _canTransition = true;
            }
            
        }
        public override void OnExit(PlayerMotion owner)
        {
            GameLoop.Instance.Hitbox.SetActive(false);
            if (owner.Param.IsLion)
            {
                owner.Param.IsLion = false;
            }
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var lionAnim = new FSMCondition<PlayerMotion>(m => m.Param.IsLion && _canTransition);
            var hurtAnim = new FSMCondition<PlayerMotion>(m => m.Param.IsSpecialHurt && _canTransition);
            var idleAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude < 0.05f && _isFinished);
            var moveAnim = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1 && _canTransition);
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.BoilTrigger.Peek() && _canTransition);
            AddCondition(lionAnim, StringConstants.AnimName.LionSmash);
            AddCondition(hurtAnim,StringConstants.AnimName.SpecialHurt);
            AddCondition(idleAnim, StringConstants.AnimName.Idle);
            AddCondition(moveAnim, StringConstants.AnimName.Move);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
        }
       
        
    }
}

