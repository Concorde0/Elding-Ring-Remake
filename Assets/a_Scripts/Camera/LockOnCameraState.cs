using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RPG.CameraSystem
{
    public class LockOnCameraState : ICameraState
    {
        private readonly CinemachineVirtualCamera _lockCam;
        private readonly CinemachineFreeLook _freeLook;
        private readonly Transform _playerFollow;
        private readonly int _lockPriority;
        private readonly int _freeLookPriority;

        private Transform _currentTarget;

        public LockOnCameraState(CinemachineVirtualCamera lockCam, CinemachineFreeLook freeLook, Transform playerFollow,
            int lockPriority = 100, int freeLookPriority = 10)
        {
            _lockCam = lockCam;
            _freeLook = freeLook;
            _playerFollow = playerFollow;
            _lockPriority = lockPriority;
            _freeLookPriority = freeLookPriority;
        }

        public void Tick()
        {
            
        }

        public void Enter()
        {
            if (_lockCam == null) return;
            _lockCam.gameObject.SetActive(true);
            _lockCam.Priority = _lockPriority;
            if (_freeLook != null)
            {
                _freeLook.Priority = _freeLookPriority;
            }
        }

        public void Exit()
        {
            if (_lockCam == null) return;
            _lockCam.Priority = _freeLookPriority - 1;
        }

        public void LockTo(Transform target)
        {
            _currentTarget = target;
            if (_lockCam == null) return;
            if (_playerFollow != null)
            {
                _lockCam.Follow = _playerFollow;
            }
            _lockCam.LookAt = _currentTarget;

            // composer参数可调整
            var composer = _lockCam.GetCinemachineComponent<CinemachineComposer>();
            if (composer != null)
            {
                composer.m_DeadZoneWidth = 0f;
                composer.m_DeadZoneHeight = 0f;
                composer.m_SoftZoneWidth = 0f;
                composer.m_SoftZoneHeight = 0f;
                composer.m_ScreenX = 0.5f;
                composer.m_ScreenY = 0.45f;
            }

            Enter();
        }

        public void Unlock()
        {
            _currentTarget = null;
            Exit();
        }

        public Transform GetCurrentTarget() => _currentTarget;
    }
}
