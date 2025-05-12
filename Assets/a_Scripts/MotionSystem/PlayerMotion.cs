using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.MotionSystem
{
    //核心编排层
    public class PlayerMotion
    {
        public PlayerAI AI { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerParam Param { get; private set; }
        public PlayerMotor Motor { get; private set; }
        public PlayerAnim Anim { get; private set; }
        public Transform Model { get; private set; }

        public PlayerMotion(Transform model, AnimationClip[] clips)
        {
            Model = model;
            Anim = new PlayerAnim(this,clips);
            Input = new PlayerInput();
            Motor = new PlayerMotor(this);
            AI = new PlayerAI(this);
        }

        public void Update()
        {
            AI.Update();
        }

        public void Stop()
        {
            Anim.Stop();
        }
    }
}

