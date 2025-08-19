using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.CameraSystem;
using RPG.MotionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class FreeLookCameraController : ICameraState
    {
        private CinemachineFreeLook _camera;
        public FreeLookCameraController(CameraResources camera)
        {
            _camera = camera.cinemachineFreeLook;
            
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

