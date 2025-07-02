using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;

namespace RPG.InputSystem
{
    public class InputBuffer
    {
        private Vector2 _lastDir = Vector2.zero;
        private float _lastTime = -Mathf.Infinity;
        private readonly float _window = 0.15f;

        public void Update(Vector2 currentDir, PlayerParam param)
        {
            
            if (currentDir.sqrMagnitude > 0.1f)
            {
                if (_lastDir.sqrMagnitude > 0.1f 
                    && Vector2.Dot(_lastDir, currentDir) < 0 
                    && Time.time - _lastTime <= _window)
                {
                    param.TurnTrigger.Set();
                }
                _lastDir = currentDir;
                _lastTime = Time.time;
            }
        }
    }
}

