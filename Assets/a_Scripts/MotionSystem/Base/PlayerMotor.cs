using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.Timer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    //运动执行层
    public class PlayerMotor
    {
        
        private readonly PlayerAnim _anim;
        private readonly Transform _model;
        private readonly Transform _camera;
        private readonly PlayerParam _param;
        private readonly TimerManager _timer;
        

        public PlayerMotor(PlayerMotion motion,CameraResources cameraResources)
        {
            _anim = motion.Anim;
            _param = motion.Param;
            _model = motion.Model;
            _camera = cameraResources.cameraTransform;
            _timer = motion.Timer;

        }
        //TODO:如果逻辑复杂，需要分离这里的逻辑切换
        public void Idle()
        {
            if (_param.IsIdleBack)
            {
                _anim.TransitionTo(StringConstants.AnimName.IdleBack);
            }
            else
            {
                _anim.TransitionTo(StringConstants.AnimName.Idle);
            }
            
        }
        public void Move(Vector2 input)
        {
            const float debounceTime = 0.2f;
            
            if (_param.IsLocked)
            {
                if (_timer.IsCooldownReady(StringConstants.AnimName.LockedMove, debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.LockedMove);
            }
           
            else if (_param.Run)
            {
                if (_timer.IsCooldownReady(StringConstants.AnimName.Run, debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.Run);
            }
            
            else
            {
                if (_timer.IsCooldownReady(StringConstants.AnimName.Move, debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.Move);
            }

            _anim.SetMoveAnim(input.x, input.y);
        }
        public void Stop()
        {
            if (_param.Run)
            {
                _anim.TransitionTo(StringConstants.AnimName.RunStop);
            }
            else
            {
                _anim.TransitionTo(StringConstants.AnimName.MoveStop);
            }
        }

        public void RunTurn()
        {
            if(_param.TurnTrigger.Consume())
            {
                _anim.TransitionTo(StringConstants.AnimName.RunTurn);
            }
        }

        public void Boil()
        {
            if (_param.BoilTrigger.Peek())
            {
                _anim.TransitionTo(StringConstants.AnimName.BoilForward);
            }
            else if (_param.JumpBackwardTrigger.Peek())
            {
                _anim.TransitionTo(StringConstants.AnimName.JumpBackward);
                
            }
        }

        public void LightAttack(int comboIndex)
        {
            switch (comboIndex)
            {
                case 0: _anim.TransitionTo(StringConstants.AnimName.LightAttack1); break;
                case 1: _anim.TransitionTo(StringConstants.AnimName.LightAttack2); break;
                case 2: _anim.TransitionTo(StringConstants.AnimName.LightAttack3); break;
                default: _anim.TransitionTo(StringConstants.AnimName.LightAttack1); break;
            }
        }
        
        
        /// <summary>
        /// 仅处理根运动（位置＋动画自身的旋转）
        /// </summary>
        public void ApplyRootMotion(float3 deltaPos, quaternion deltaRot)
        {
            _model.position += (Vector3)deltaPos;
            _model.rotation = math.mul(deltaRot, _model.rotation);
        }

        /// <summary>
        /// 如果允许，用输入方向（相对于摄像机）来平滑旋转角色朝向
        /// </summary>
        public void HandleInputRotation()
        {
            Vector2 input = _param.MoveInput;
            if (input.sqrMagnitude <= 0.01f) return;

            Vector3 camF = _camera.forward; camF.y = 0; camF.Normalize();
            Vector3 camR = _camera.right;   camR.y = 0; camR.Normalize();

            Vector3 moveDir = camF * input.y + camR * input.x;
            if (moveDir.sqrMagnitude > 0.01f)
            {
                Quaternion target = Quaternion.LookRotation(moveDir);
                _model.rotation = Quaternion.Slerp(
                    _model.rotation,
                    target,
                    Time.deltaTime * _param.RotateSpeed
                );
            }
        }
    }
}


