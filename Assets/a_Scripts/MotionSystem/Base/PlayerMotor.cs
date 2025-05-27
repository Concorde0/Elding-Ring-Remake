using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    //运动执行层
    public class PlayerMotor
    {
        
        private readonly PlayerAnim _anim;

        private Transform _model;
        private PlayerParam _param;
        

        public PlayerMotor(PlayerMotion motion)
        {
            _anim = motion.Anim;
            _param = motion.Param;
            _model = motion.Model;
            
        }
        public void Idle()
        {
            _anim.TransitionTo(StringConstants.AnimName.Idle);
        }

        public void Move(Vector2 input)
        {
            if (_param.isLocked)
            {
                _anim.TransitionTo(StringConstants.AnimName.LockedMove);
            }
            else if(_param.run)
            {
                _anim.TransitionTo(StringConstants.AnimName.Run);
            }
            else
            {
                _anim.TransitionTo(StringConstants.AnimName.Move);
            }
            
            _anim.SetMoveAnim(input.x, input.y);
        }
        
        public void ApplyRootMotion(float3 deltaPos, quaternion deltaRot)
        {
            // 简单实现：直接叠加到角色Transform上
            _model.position += (Vector3)deltaPos;
            _model.rotation = math.mul(_model.rotation, deltaRot);
        }

    }
}

