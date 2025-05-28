using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JobSystem;
using RPG.AnimationSystem;
using RPG.CameraSystem;
using RPG.InputSystem;
using UnityEngine;

namespace RPG.MotionSystem
{
    //核心编排层
    public class PlayerMotion
    {
        public PlayerInputController Input{ get; private set; }
        public PlayerAI AI { get; private set; }
        public PlayerParam Param { get; private set; }
        public PlayerMotor Motor { get; private set; }
        public PlayerAnim Anim { get; private set; }
        public Transform Model { get; private set; }
        public PlayerCamera Camera { get; private set; }
        
        public RootMotionJobHandler RootMotion { get; private set; }

        public PlayerMotion(Transform model, AnimSetting setting, CameraResources camera)
        {
            Model = model;
            Param = new PlayerParam();
            Input = new PlayerInputController(Param);
            Anim = new PlayerAnim(this, setting);
            Motor = new PlayerMotor(this, camera);
            AI = new PlayerAI(this);
            //TODO：这里RootMotion的参数代表了需要处理几个角色的根运动，目前只有Player所以简单记作 1
            RootMotion = new RootMotionJobHandler(1);
            Camera = new PlayerCamera(camera, Param);
        }

        public void Start()
        {
            Input.Enable();
            Camera.Start();
        }
        

        public void Update()
        {
            // Input.Update();
            AI.Update();
            Camera.Update();
            
        }

        public void FixedUpdate()
        {
            RootCalculate();
        }

        public void Stop()
        {
            Anim.Stop();
            Input.Stop();
        }

        private void RootCalculate()
        {
            RootMotion.RecordPrevious(0, Model);
            Anim.EvaluateGraph(Time.fixedDeltaTime);
            RootMotion.RecordAndSchedule(0, Model);
            RootMotion.CompleteAndApply((index, deltaPos, deltaRot) =>
            {
                Motor.ApplyRootMotion(deltaPos, deltaRot, Param.isLocked ? RotationMode.UseRootMotion : RotationMode.UseDeltaPos);
            }); 
        }
    }
}

