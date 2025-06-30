using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.MotionSystem;
using UnityEngine;

namespace RPG.CameraSystem
{
    public class FreeLookCameraController : ICameraState
    {
        private CinemachineFreeLook _camera;
        private PlayerParam _param;
        public FreeLookCameraController(CameraResources camera, PlayerParam param)
        {
            _camera = camera.cinemachineFreeLook;
            _param = param;
            
            camera.cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
            camera.cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
        }
        
        public void Tick()
        {
        }

        public void Enter()
        {
            _camera.gameObject.SetActive(true);
        }

        public void Exit()
        {
            _camera.gameObject.SetActive(false);
        }
    }
}

