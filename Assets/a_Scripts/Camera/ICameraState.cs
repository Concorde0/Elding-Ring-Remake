using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraSystem
{
    public interface ICameraState 
    {
        public void Tick(float deltaTime);
        void Enter();
        void Exit();
    }
}

