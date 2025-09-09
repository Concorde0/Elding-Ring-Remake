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
        private readonly FSMStateTracker _tracker;
        //TODO:把AddState和AddCondition放在FSMState里面让他自己注册
        public PlayerAI(PlayerMotion motion)
        {
            _fsm = new BaseFSM<PlayerMotion>(motion);
            
            _fsm.AddState(StringConstants.AnimName.Idle, new PlayerIdleState());
            _fsm.AddState(StringConstants.AnimName.Move, new PlayerMoveState());
            _fsm.AddState(StringConstants.AnimName.MoveStop, new PlayerStopState());
            _fsm.AddState(StringConstants.AnimName.BoilForward, new PlayerBoilState());
            _fsm.AddState(StringConstants.AnimName.LightAttack1, new PlayerAttackState());
            _fsm.AddState(StringConstants.AnimName.RunTurn,new PlayerRunTurnState());
            _fsm.AddState(StringConstants.AnimName.SpecialHurt,new PlayerHurtState());
            _fsm.AddState(StringConstants.AnimName.Execution,new PlayerExecutionState());
            _fsm.SetDefault(StringConstants.AnimName.Idle);
            //TODO:Condition的逻辑切换
            
            _tracker = new FSMStateTracker();
            _tracker.AttachTo(_fsm);
        }

        public void Update()
        {
            _fsm.Update();
        }

        public string GetLastStateName()
        {
            return _tracker.Previous;
        }

       
    }
}

