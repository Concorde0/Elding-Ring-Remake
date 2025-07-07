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
            owner.Motor.Idle();
            owner.AI.GetLastStateName();
        }

        public override void OnUpdate(PlayerMotion owner)
        {
           Debug.Log("Player Idle State");
        }

        public override void RegisterTransitions(BaseFSM<PlayerMotion> fsm)
        {
            var moveInput = new FSMCondition<PlayerMotion>(m => m.Param.MoveInput.sqrMagnitude >= 0.1f);
            var boilAnim = new FSMCondition<PlayerMotion>(m => m.Param.BoilTrigger.Peek() || m.Param.JumpBackwardTrigger.Peek());
            var attackAnim = new FSMCondition<PlayerMotion>(m => m.Param.AttackTrigger.Peek());
            AddCondition(boilAnim,StringConstants.AnimName.BoilForward);
            AddCondition(moveInput, StringConstants.AnimName.Move);
            AddCondition(attackAnim, StringConstants.AnimName.LightAttack1);
        }
    }
}

