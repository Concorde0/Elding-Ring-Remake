using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Timer
{
    public static class TimerExtensions
    {
        public static bool IsCooldownReady(this TimerManager timers, string key, float duration)
        {
            if (!timers.Exists(key) || timers.IsFinished(key))
            {
                timers.Start(key, duration);
                return true;
            }

            return false;
        }
    }
}

