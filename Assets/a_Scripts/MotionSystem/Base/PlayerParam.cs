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
        public bool Boil;
        public bool JumpBackward;
        public bool canAttack = true;
        
        public readonly float RotateSpeed = 10f;
        private readonly Queue<AttackCommand> attackQueue = new();
        

        public void PushAttack()
        {
            attackQueue.Enqueue(new AttackCommand());
        }

        public bool TryConsumeAttack()
        {
            if (attackQueue.Count > 0)
            {
                attackQueue.Dequeue();
                return true;
            }

            return false;
        }

        public void ClearQueue()
        {
            attackQueue.Clear();
        }

        public int GetQueueCount()
        {
            return attackQueue.Count;
        }
        
        public class AttackCommand
        {
            public float time;
            public AttackCommand()
            {
                time = Time.time;
            }
        }

        //TODO:之后做完camera的敌人锁定之后，把敌人的position写到parma中，激活这个函数
        // public Transform lockedTarget;  // 锁定的目标

        // private Dictionary<string , AnimBehaviour> _anims;
        //
        // public void AddAnim(string animName, AnimBehaviour animBehaviour)
        // {
        //     _anims ??= new Dictionary<string , AnimBehaviour>();
        //     _anims.Add(animName , animBehaviour);
        // }
        //
        // public bool IsAnimEnd(string name, float threshold = 0.1f)
        // {
        //     if (_anims.TryGetValue(name, out var anim))
        //     {
        //         return anim.enable && anim.remainTime <= threshold;
        //     }
        //     return false;
        // }

    }
}



