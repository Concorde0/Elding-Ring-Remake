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
        private float _xSensitivity = 300f;
        private float _ySensitivity = 2f;
        public FreeLookCameraController(CameraResources camera, PlayerParam param)
        {
            _camera = camera.cinemachineFreeLook;
            _param = param;
            
            camera.cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
            camera.cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
        }
        
        public void Tick(float deltaTime)
        {
            Vector2 look = _param.lookInput;
            _camera.m_XAxis.Value += look.x * _xSensitivity * deltaTime;
            _camera.m_YAxis.Value += look.y * _ySensitivity * deltaTime;
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

