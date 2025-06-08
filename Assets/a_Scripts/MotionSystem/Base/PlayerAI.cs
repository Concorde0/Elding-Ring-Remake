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
            
            _fsm.AddState(StringConstants.AnimName.Idle, new PlayerIdleState());
            _fsm.AddState(StringConstants.AnimName.Move, new PlayerMoveState());
            _fsm.AddState(StringConstants.AnimName.MoveStop, new PlayerStopState());
            _fsm.AddState(StringConstants.AnimName.BoilForward, new PlayerBoilState());
            _fsm.SetDefault(StringConstants.AnimName.Idle);
            //TODO:Condition的逻辑切换
            
           

            // 绑定到状态
            
            
        }

        public void Update()
        {
            _fsm.Update();
        }

       
    }
}

