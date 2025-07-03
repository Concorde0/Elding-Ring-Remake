using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConstants : Singleton<StringConstants>
{
    public class  AnimName
    {
        public const string Idle = "Idle";
        public const string Move = "Move";
        public const string LockedMove = "LockedMove";
        public const string Run = "Run";
        public const string RunStop = "RunStop";
        public const string MoveStop = "MoveStop";
        public const string LockedMoveStop = "LockedMoveStop";
        public const string RunTurn = "RunTurn";
        public const string Jump = "Jump";
        public const string JumpBackward = "JumpBackward";
        public const string MoveStopForward = "MoveStopForward";
        public const string MoveStopBackward = "MoveStopBackward";
        public const string MoveStopLeftward = "MoveStopLeftward";
        public const string MoveStopRightward = "MoveStopRightward";
        public const string BoilForward = "BoilForward";
        public const string BoilBackward = "BoilBackward";
        public const string BoilLeftward = "BoilLeftward";
        public const string BoilRightward = "BoilRightward";
        public const string LightAttack1 = "LightAttack1";
        public const string LightAttack2 = "LightAttack2";
        public const string LightAttack3 = "LightAttack3";
        
    }

    public class TimerName
    {
        public const string MoveToStop = "MoveToStop";
        public const string MoveDelayTransition = "MoveDelayTransition";
        public const string SpacePerform = "SpacePerform";
        public const string BoilTime = "BoilTime";
        public const string RunTurnDuration = "RunTurnDuration";
        public const string JumpBackwardTime = "JumpBackwardTime";
        public const string LightAttackTime1 = "LightAttackTime1";
        public const string LightAttackTime2 = "LightAttackTime2";
        public const string LightAttackTime3 = "LightAttackTime3";
        public const string SpaceHold = "SpaceHold";
    }
}
