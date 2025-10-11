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
        public bool Run;
        // public bool CheckLocked;
        public bool IsLocked;
        public bool IsInBoil;
        public bool IsIdleBack;
        public bool IsSpecialHurt;
        public bool IsLion;
        public readonly float RotateSpeed = 15f;
        public readonly TriggerParam CheckLocked = new TriggerParam();
        public readonly TriggerParam JumpBackwardTrigger = new TriggerParam();
        public readonly TriggerParam BoilTrigger = new TriggerParam();
        public readonly TriggerParam AttackTrigger = new TriggerParam();
        public readonly TriggerParam TurnTrigger = new TriggerParam();
        
       
        

    }
}



