using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Timer
{
    public class Timer
    {
        public float Duration{ get; private set; }
        public float StartTime{ get; private set; }
        public bool IsRunning => Time.time >= StartTime + Duration;
    
        public Timer(float duration)
        {
            Duration = duration;
            StartTime = Time.time;
        }
    
        public void ReStart(float? newDuration = null)
        {
            Duration = newDuration ?? Duration;
            StartTime = Time.time;
        }
        
        public float Elapsed => Time.time - StartTime;
        public bool IsFinished => Elapsed >= Duration;
    }
    
    public class TimerManager
    {
        private Dictionary<string, Timer> _timers = new();
    
        public void Start(string key, float duration)
        {
            if (_timers.ContainsKey(key))
            {
                _timers[key].ReStart(duration);
            }
            else
            {
                _timers[key] = new Timer(duration);
            }
        }
    
        public bool IsFinished(string key)
        {
            return _timers.TryGetValue(key, out var timer) && timer.IsFinished;
        }
    
        public float GetElapsed(string key)
        {
            return _timers.TryGetValue(key, out var timer) ? timer.Elapsed : 0f;
        }
    
        public void Restart(string key)
        {
            if (_timers.TryGetValue(key, out var timer))
            {
                timer.ReStart(null);
            }
        }
    
        public void Remove(string key)
        {
            _timers.Remove(key);
        }
    
        public bool Exists(string key)
        {
            return _timers.ContainsKey(key);
        }
    
    }
}
