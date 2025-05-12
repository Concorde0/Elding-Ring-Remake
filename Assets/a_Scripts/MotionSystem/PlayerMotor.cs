using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    //运动执行层
    public class PlayerMotor
    {
        
        private PlayerAnim _anim;

        public PlayerMotor(PlayerMotion motion)
        {
            _anim = motion.Anim;
        }
        
        public void Idle()
        {
            _anim.TransitionTo("Idle");
        }
    }
}

