using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;


namespace RPG.MotionSystem
{
    //状态机层
    public class PlayerAI
    {
        private readonly BaseFSM<PlayerMotion> _fsm;

        public PlayerAI(PlayerMotion motion)
        {
            _fsm = new BaseFSM<PlayerMotion>(motion);

            var idle = new FSMState<PlayerMotion>();
            idle.BindEnterAction(m => m.Motor.Idle());
            _fsm.AddState("Idle",idle);
            _fsm.SetDefault("Idle");

            var move = new FSMState<PlayerMotion>();
            move.BindUpdateAction(m => m.Motor.Move(m.Param.moveInput));
            _fsm.AddState("Move",move);
            
            
            FSMCondition<PlayerMotion> enterMove = 
                new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude >= 0.1f);
            
            FSMCondition<PlayerMotion> exitMove = 
                new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.05f);

            // 绑定到状态
            idle.AddCondition(enterMove, "Move");
            move.AddCondition(exitMove, "Idle");
            
        }

        public void Update()
        {
            _fsm.Update();
            
        }
    }
}

