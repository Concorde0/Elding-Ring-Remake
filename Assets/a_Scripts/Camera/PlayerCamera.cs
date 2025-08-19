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
        
        public PlayerCamera(CameraResources camera) 
        {
            _freeLook = new FreeLookCameraController(camera);
            _currentState = _freeLook;
        }
        
        public void Tick()
        {
            _currentState?.Tick();
        }

        public void SwitchToFreeLook()
        {
            _currentState = _freeLook;
        } 

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

