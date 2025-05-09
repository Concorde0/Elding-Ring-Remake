using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inputsystem
{
    public enum ActionType
    {
    
    }

    public class PlayerInputSystem : MonoBehaviour
    {
        private PlayerInput _playerInput;
    
        [SerializeField]
        private Vector2 _inputDirection;
        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void Update()
        {
            GetInputDirection();
        }

        private void FixedUpdate()
        {
        
        }

        private void GetInputDirection()
        {
            _inputDirection = _playerInput.GamePlay.Move.ReadValue<Vector2>();
        }
    }
}


