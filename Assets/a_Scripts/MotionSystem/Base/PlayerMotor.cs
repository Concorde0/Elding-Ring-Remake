using System.Collections;
using System.Collections.Generic;
using RPG.Timer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    public enum RotationMode
    {
        UseRootMotion,     // 完全使用动画旋转（默认）
        UseDeltaPos,       // 从 deltaPos 推导方向
        UseLookAtTarget,   // 朝向锁定目标
        None               // 不旋转（保留当前朝向）
    }
    //运动执行层
    public class PlayerMotor
    {
        
        private readonly PlayerAnim _anim;

        private Transform _model;
        private PlayerParam _param;
        private Transform _camera;
        private TimerManager _timer;
        

        public PlayerMotor(PlayerMotion motion,CameraResources cameraResources)
        {
            _anim = motion.Anim;
            _param = motion.Param;
            _model = motion.Model;
            _camera = cameraResources.cameraTransform;
            _timer = new TimerManager();

        }
        //TODO:如果逻辑复杂，需要分离这里的逻辑切换
        public void Idle()
        {
            _anim.TransitionTo(StringConstants.AnimName.Idle);
        }
        
        public void Move(Vector2 input)
        {
            const float debounceTime = 0.2f;
            
            if (_param.isLocked)
            {
                if (_timer.IsCooldownReady("LockedMove", debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.LockedMove);
            }
           
            else if (_param.run)
            {
                if (_timer.IsCooldownReady("Run", debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.Run);
            }
            
            else
            {
                if (_timer.IsCooldownReady("Move", debounceTime))
                    _anim.TransitionTo(StringConstants.AnimName.Move);
            }

            _anim.SetMoveAnim(input.x, input.y);
        }

        public void Stop()
        {
            if (_param.run)
            {
                _anim.TransitionTo(StringConstants.AnimName.RunStop);
            }
            else
            {
                _anim.TransitionTo(StringConstants.AnimName.MoveStop);
            }
        }
        
        
        
        public void ApplyRootMotion(float3 deltaPos, quaternion deltaRot, RotationMode rotationMode)
        {
            _model.position += (Vector3)deltaPos;

            switch (rotationMode)
            {
                case RotationMode.UseRootMotion:
                    _model.rotation = math.mul(_model.rotation, deltaRot);
                    break;

                case RotationMode.UseDeltaPos:
                {
                    Vector2 input = _param.moveInput;

                    Vector3 camForward = _camera.forward;
                    Vector3 camRight = _camera.right;
                    camForward.y = 0;
                    camRight.y = 0;
                    camForward.Normalize();
                    camRight.Normalize();

                    Vector3 moveDir = camForward * input.y + camRight * input.x;

                    if (moveDir.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(moveDir);
                        _model.rotation = Quaternion.Slerp(_model.rotation, targetRot,Time.deltaTime * _param.rotateSpeed);
                    }
                    break;
                }
                //TODO:之后做完camera的敌人锁定之后，把敌人的position写到parma中，激活这个函数

                // case RotationMode.UseLookAtTarget:
                //     if (_param.lockedTarget != null)
                //     {
                //         Vector3 toTarget = _param.lockedTarget.position - _model.position;
                //         toTarget.y = 0;
                //         if (toTarget.sqrMagnitude > 0.001f)
                //         {
                //             Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
                //             _model.rotation = Quaternion.Slerp(
                //                 _model.rotation,
                //                 targetRot,
                //                 Time.deltaTime * _param.rotateSpeed
                //             );
                //         }
                //     }
                //     break;

                case RotationMode.None:
                default:
                    // 什么也不做
                    break;
            }
        }
        
    }
}

