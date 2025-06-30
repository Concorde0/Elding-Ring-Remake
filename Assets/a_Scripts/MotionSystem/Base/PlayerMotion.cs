using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JobSystem;
using RPG.AnimationSystem;
using RPG.CameraSystem;
using RPG.FSM;
using RPG.InputSystem;
using RPG.Timer;
using UnityEngine;

namespace RPG.MotionSystem
{
    //核心编排层
    public class PlayerMotion
    {
        //TODO: 某些Class需要换位置，后期优化
        public PlayerInputController Input{ get; private set; }
        public PlayerAI AI { get; private set; }
        public PlayerParam Param { get; private set; }
        public PlayerMotor Motor { get; private set; }
        public PlayerAnim Anim { get; private set; }
        public Transform Model { get; private set; }
        public PlayerCamera Camera { get; private set; }
        public RootMotionJobHandler RootMotion { get; private set; }
        public TimerManager Timer { get; private set; }

        public PlayerMotion(Transform model, AnimSetting setting, CameraResources camera)
        {
            Model = model;
            Param = new PlayerParam();
            Anim = new PlayerAnim(this, setting);
            Motor = new PlayerMotor(this, camera);
            AI = new PlayerAI(this);
            //TODO：这里RootMotion的参数代表了需要处理几个角色的根运动，目前只有Player所以简单记作 1
            RootMotion = new RootMotionJobHandler(1);
            Timer = new TimerManager();
            Input = new PlayerInputController(Param, Timer);
            Camera = new PlayerCamera(camera, Param);
        }

        public void Start()
        {
            Input.Enable();
            Camera.Start();
        }
        

        public void Update()
        {
            AI.Update();
            Camera.Update();
            Input.Update();
            
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
            Anim.EvaluateGraph(Time.fixedDeltaTime);
            RootMotion.RecordAndSchedule(0, Model);
            RootMotion.CompleteAndApply((index, deltaPos, deltaRot) =>
            {
                Motor.ApplyRootMotion(deltaPos, deltaRot, Param.IsLocked ? RotationMode.UseRootMotion : RotationMode.UseDeltaPos);
            }); 
            RootMotion.RecordPrevious(0, Model);
        }
    }
}

