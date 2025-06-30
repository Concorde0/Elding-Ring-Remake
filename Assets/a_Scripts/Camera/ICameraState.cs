using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraSystem
{
    public interface ICameraState 
    {
        public void Tick();
        void Enter();
        void Exit();
    }
}

