using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using RPG.MotionSystem.States;
using UnityEngine;


namespace RPG.MotionSystem
{
    //状态机层
    public class PlayerAI
    {
        private readonly BaseFSM<PlayerMotion> _fsm;
        
        //TODO:把AddState和AddCondition放在FSMState里面让他自己注册
        public PlayerAI(PlayerMotion motion)
        {
            _fsm = new BaseFSM<PlayerMotion>(motion);
            
            var idle = new PlayerIdleState();
            _fsm.AddState(StringConstants.AnimName.Idle,idle);
            _fsm.SetDefault(StringConstants.AnimName.Idle);

            var move = new PlayerMoveState();
            _fsm.AddState(StringConstants.AnimName.Move,move);
            
            var stop = new PlayerStopState();
            _fsm.AddState(StringConstants.AnimName.MoveStop,stop);
            
            
            FSMCondition<PlayerMotion> enterMove = 
                new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude >= 0.1f);
            FSMCondition<PlayerMotion> exitMove = 
                new FSMCondition<PlayerMotion>(m => m.Param.moveInput.sqrMagnitude < 0.05f);

            // 绑定到状态
            idle.AddCondition(enterMove, StringConstants.AnimName.Move);
            move.AddCondition(exitMove, StringConstants.AnimName.Idle);
            // stop.AddCondition();
            
        }

        public void Update()
        {
            _fsm.Update();
        }

       
    }
}

