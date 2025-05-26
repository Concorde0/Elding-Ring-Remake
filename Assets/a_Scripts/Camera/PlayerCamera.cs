using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.MotionSystem;
using UnityEngine;


namespace RPG.CameraSystem
{
    
    public class PlayerCamera
    {
        private ICameraState _currentState;
        
        private readonly FreeLookCameraController _freeLook;
        // private PlayerParam _param;
        
        public PlayerCamera(CameraResources camera, PlayerParam param) 
        {
            _freeLook = new FreeLookCameraController(camera, param);
            _currentState = _freeLook;
        }
        
        public void Tick(float deltaTime) => _currentState?.Tick(deltaTime);
        
        public void SwitchToFreeLook() => _currentState = _freeLook;

        public void Start()
        {
            _freeLook.Enter();
        }

        public void Enable()
        {
            
        }

        public void Update()
        {
            
        }

        public void Stop()
        {
            _freeLook.Exit();
        }
    }   
}

