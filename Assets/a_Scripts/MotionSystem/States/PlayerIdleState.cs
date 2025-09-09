using System.Collections;
using System.Collections.Generic;
using LitJson;
using RPG.FSM;
using UnityEngine;

namespace RPG.MotionSystem.States
{
    public class PlayerIdleState : FSMState<PlayerMotion>
    {
        public override void OnEnter(PlayerMotion owner)
        {
            if (owner.AI.GetLastStateName() == StringConstants.AnimName.RunTurn)
            {
                owner.Param.IsIdleBack = true;
            } 
            owner.Motor.Idle();
        }

        public override void OnUpdate(PlayerMotion owner)
        {
            
        }

        public override void OnExit(PlayerMotion owner)
        {
            owner.Param.IsIdleBack = false;
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var moveInput = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1f);
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.BoilTrigger.Peek() || m.Param.JumpBackwardTrigger.Peek());
            var attackAnim = new FSMCondition<PlayerMotion>(m => m.Param.AttackTrigger.Peek());
            var hurtAnim = new FSMCondition<PlayerMotion>(m => m.Param.IsSpecialHurt);
            AddCondition(hurtAnim,StringConstants.AnimName.SpecialHurt);
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
            AddCondition(moveInput, StringConstants.AnimName.Move);
            AddCondition(attackAnim, StringConstants.AnimName.LightAttack1);
        }
    }
}

