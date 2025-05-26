using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConstants : Singleton<StringConstants>
{
    public class AnimName
    {
        public const string Idle = "Idle";
        public const string Move = "Move";
        public const string LockedMove = "LockedMove";
        public const string Run = "Run";
        public const string RunStop = "RunStop";
        public const string Jump = "Jump";
        public const string BoilForward = "BoilForward";
        public const string BoilBackward = "BoilBackward";
        public const string BoilLeftward = "BoilLeftward";
        public const string BoilRightward = "BoilRightward";
        public const string NormalAttack1 = "NormalAttack1";
        public const string NormalAttack2 = "NormalAttack2";
        public const string NormalAttack3 = "NormalAttack3";
    }
}
