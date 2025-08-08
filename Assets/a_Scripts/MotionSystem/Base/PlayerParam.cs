using System;
using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using UnityEngine;


namespace RPG.MotionSystem
{
    
    //动画配置层
    public class PlayerParam
    {
        public Vector2 MoveInput;
        public Vector2 LookInput;
        public bool Run;
        public bool IsLocked;
        public bool IsInBoil;
        public bool IsIdleBack;
        public readonly float RotateSpeed = 15f;
        public readonly TriggerParam JumpBackwardTrigger = new TriggerParam();
        public readonly TriggerParam BoilTrigger = new TriggerParam();
        public readonly TriggerParam AttackTrigger = new TriggerParam();
        public readonly TriggerParam TurnTrigger = new TriggerParam();
        


        

    }
}



