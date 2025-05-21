using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConstants : Singleton<StringConstants>
{
    public class AnimName
    {
        public const string Move = "Move";
        public const string LockedMove = "LockedMove";
        public const string Run = "Run";
        public const string Jump = "Jump";
        public const string Idle = "Idle";
    }
}
