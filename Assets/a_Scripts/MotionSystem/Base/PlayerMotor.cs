using System;
using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.Timer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    // 运动执行层：增加最小化的锁定移动逻辑
    public class PlayerMotor
    {
        private readonly PlayerAnim _anim;
        private readonly Transform _model;
        private readonly Transform _camera;
        private readonly PlayerParam _param;
        private readonly TimerManager _timer;

        
        private PlayerGrounder _grounder;

        private Transform _lockTarget;
        
        private bool _hasPlayedLionSmash;

        public PlayerMotor(PlayerMotion motion, CameraResources cameraResources)
        {
            _anim = motion.Anim;
            _param = motion.Param;
            _model = motion.Model;
            _camera = cameraResources.cameraTransform;
            _timer = motion.Timer;
            
            _grounder = new PlayerGrounder(_model, LayerMask.GetMask("Ground"));
            
        }

        public void OnLockTargetChanged(Transform target)
        {
            _lockTarget = target;
        }
        
        public void Idle()
        {
            _anim.TransitionTo(_param.IsIdleBack ? StringConstants.AnimName.IdleBack : StringConstants.AnimName.Idle);
        }

        public void Move(Vector2 input)
        {
            const float debounceTime = 0.2f;

            if (_param.IsLocked)
            {
                if (_timer.IsCooldownReady(StringConstants.AnimName.LockedMove, debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.LockedMove);
            }
            else
            {
                if (_param.Run)
                {
                    if (_timer.IsCooldownReady(StringConstants.AnimName.Run, debounceTime))
                        _anim.TransitionTo(StringConstants.AnimName.Run);
                }
                else
                {
                    if (_timer.IsCooldownReady(StringConstants.AnimName.Move, debounceTime))
                        _anim.TransitionTo(StringConstants.AnimName.Move);
                }
            }

            // 依旧把输入传给动画（四向 Blend），动画决定视觉播放
            _anim.SetMoveAnim(input.x, input.y);
        }

        public void Stop()
        {
            _anim.TransitionTo(_param.Run ? StringConstants.AnimName.RunStop : StringConstants.AnimName.MoveStop);
        }

        public void RunTurn()
        {
            if (_param.TurnTrigger.Consume()) _anim.TransitionTo(StringConstants.AnimName.RunTurn);
        }

        public void Boil()
        {
            if (_param.IsLocked && _param.BoilTrigger.Peek())
            {
                _anim.TransitionTo(StringConstants.AnimName.LockedBoil);
            }
            if (!_param.IsLocked && _param.BoilTrigger.Peek())
            {
                _anim.TransitionTo(StringConstants.AnimName.BoilForward);
            }
            else if (_param.JumpBackwardTrigger.Peek())
            {
                _anim.TransitionTo(StringConstants.AnimName.JumpBackward);
            }
        }
        
        
        public void LionSmash()
        {
            if (_param.IsLion && !_hasPlayedLionSmash)
            {
                _anim.TransitionTo(StringConstants.AnimName.LionSmash);
                _hasPlayedLionSmash = true;
            }
        }

        public void Execution()
        {
            _anim.TransitionTo(StringConstants.AnimName.Execution);
        }

        public void ResetLionSmash()
        {
            _hasPlayedLionSmash = false;
        }
        
        public void LightAttack(int comboIndex)
        {
            switch (comboIndex)
            {
                case 0:
                    _anim.TransitionTo(StringConstants.AnimName.LightAttack1);
                    break;
                case 1:
                    _anim.TransitionTo(StringConstants.AnimName.LightAttack2);
                    break;
                case 2:
                    _anim.TransitionTo(StringConstants.AnimName.LightAttack3);
                    break;
                default:
                    _anim.TransitionTo(StringConstants.AnimName.LightAttack1);
                    break;
            }
        }
        
        public void Hurt()
        {
            if (_param.IsSpecialHurt)
            {
                _anim.TransitionTo(StringConstants.AnimName.SpecialHurt);
            }
        }
        
        public void ApplyRootMotion(float3 deltaPos, quaternion deltaRot)
        {
            deltaPos += (float3)Physics.gravity * Time.fixedDeltaTime;
            if (!_param.IsLocked)
            {
                _model.position += (Vector3)deltaPos;
            }
            _grounder.Grounding();
            _grounder.StepClimb();
            
        }
        
        
        
        public void HandleInputRotation()
        {
            Vector2 input = _param.MoveInput;
            if (input.sqrMagnitude <= 0.01f) return;

            Vector3 camF = _camera.forward;
            camF.y = 0;
            camF.Normalize();
            Vector3 camR = _camera.right;
            camR.y = 0;
            camR.Normalize();

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
        
        public void HandleLockRotation()
        {
            if (_lockTarget == null)
            {
                Debug.LogWarning("null");
                _param.IsLocked = false;
                return;
            }

            Vector3 moveDir = _lockTarget.position - _model.position;
            moveDir.y = 0f; // 保持水平朝向

            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion want = Quaternion.LookRotation(moveDir);
                _model.rotation = Quaternion.Slerp(
                    _model.rotation,
                    want,
                    Time.deltaTime * _param.RotateSpeed
                );
            }
        }
    }
}
