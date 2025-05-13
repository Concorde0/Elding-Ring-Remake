using System.Collections;
using System.Collections.Generic;
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
            //TODO:需要切换多种移动的动画逻辑
            _anim.TransitionTo("Move");
            
        }
    }
}

