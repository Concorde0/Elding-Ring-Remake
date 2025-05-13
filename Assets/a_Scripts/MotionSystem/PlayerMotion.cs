using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using UnityEngine;

namespace RPG.MotionSystem
{
    //核心编排层
    public class PlayerMotion
    {
        public PlayerInputController Input{get;private set;}
        public PlayerAI AI { get; private set; }
        public PlayerParam Param { get; private set; }
        public PlayerMotor Motor { get; private set; }
        public PlayerAnim Anim { get; private set; }
        public Transform Model { get; private set; }

        public PlayerMotion(Transform model, AnimSetting setting)
        {
            Model = model;
            Param = new PlayerParam();
            Input = new PlayerInputController(Param);
            Anim = new PlayerAnim(this,setting);
            Motor = new PlayerMotor(this);
            AI = new PlayerAI(this);
        }

        public void Start()
        {
            Input.Enable();
        }
        

        public void Update()
        {
            Input.Update();
            AI.Update();
        }

        public void Stop()
        {
            Anim.Stop();
            Input.Stop();
        }
    }
}

