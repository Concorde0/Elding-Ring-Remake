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
            
            
            
            // var moveInput = new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude >= 0.01f);
            // idle.AddCondition(moveInput,"Move");
            // move.AddCondition(!moveInput,"Idle");
            
            var moveCondition = new CompoundCondition<PlayerMotion>()
                .AddEnterCondition(m => m.Param.moveInput.sqrMagnitude >= 0.1f) // 进入条件：输入强度 > 0.1
                .AddExitCondition(m => m.Param.moveInput.sqrMagnitude < 0.05f)   // 退出条件：输入强度 < 0.05
                .Build();
            var moveFSMCondition = new FSMCondition<PlayerMotion>(moveCondition);
            idle.AddCondition(moveFSMCondition,"Move");
            move.AddCondition(new FSMCondition<PlayerMotion>(m => !moveCondition(m)),"Idle");
            
        }

        public void Update()
        {
            _fsm.Update();
            
        }
    }
}

