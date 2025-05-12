using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;


namespace RPG.MotionSystem
{
    //状态机层
    public class PlayerAI
    {
        private BaseFSM<PlayerMotion> _fsm;

        public PlayerAI(PlayerMotion motion)
        {
            _fsm = new BaseFSM<PlayerMotion>(motion);

            var idle = new FSMState<PlayerMotion>();
            idle.BindEnterAction(m => m.Motor.Idle());
            _fsm.AddState("Idle",idle);
            _fsm.SetDefault("Idle");
        }

        public void Update()
        {
            _fsm.Update();
        }
    }
}

