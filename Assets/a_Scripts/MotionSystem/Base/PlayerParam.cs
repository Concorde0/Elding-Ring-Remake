using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool Boil;
        public bool JumpBackward;
        
        //TODO:之后做完camera的敌人锁定之后，把敌人的position写到parma中，激活这个函数
        // public Transform lockedTarget;  // 锁定的目标
        public readonly float RotateSpeed = 10f;
        
    }
}



