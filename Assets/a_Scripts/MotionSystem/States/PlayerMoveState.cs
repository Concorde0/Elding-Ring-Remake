using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerMoveState : FSMState<PlayerMotion>
    {
        public PlayerMoveState()
        {
            
        }
        public override void OnEnter(PlayerMotion owner)
        {
            base.OnEnter(owner);
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Move(owner.Param.moveInput);
        }
        
        
    }
}

