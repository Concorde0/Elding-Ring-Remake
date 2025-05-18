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
        private Rigidbody _rigidbody;
        private PlayerParam _param;
        

        public PlayerMotor(PlayerMotion motion)
        {
            _anim = motion.Anim;
            _param = motion.Param;
            _model = motion.Model;
            _rigidbody = _model.GetComponent<Rigidbody>();
            
        }
        public void Idle()
        {
            _anim.TransitionTo("Idle");
        }

        public void Move(Vector2 input)
        {
            _anim.TransitionTo("Move");
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

