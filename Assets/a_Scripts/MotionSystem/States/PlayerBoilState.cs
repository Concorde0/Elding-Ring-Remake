using System.Collections;
using System.Collections.Generic;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerBoilState : FSMState<PlayerMotion>
    {
       
        public override void OnEnter(PlayerMotion owner)
        {
            base.OnEnter(owner);
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            owner.Motor.Boil();
        }
        
        public override void OnExit(PlayerMotion owner)
        {
            base.OnExit(owner);
        }
        
        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.Boil);
            var jumpBackwardAnim = new FSMCondition<PlayerMotion>(m => m.Param.JumpBackward);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
            AddCondition(jumpBackwardAnim,StringConstants.AnimName.JumpBackward);
        }
    }
}

