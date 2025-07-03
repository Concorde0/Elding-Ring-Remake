using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using RPG.Timer;
using UnityEngine;
using UnityEngine.InputSystem;


namespace RPG.InputSystem
{
    public class PlayerInputController
    {
        private readonly PlayerInput _input;
        private readonly PlayerParam _param;
        private readonly TimerManager _timerManager;
        private readonly InputBuffer _inputBuffer = new InputBuffer();

        
        private bool _spaceHeld;
        public PlayerInputController(PlayerParam param, TimerManager timerManager)
        {
            _param = param;
            _timerManager = timerManager;
            _input = new PlayerInput();

            _input.GamePlay.Attack.performed += OnAttackPerformed;
            
            _input.GamePlay.Look.performed += ctx => _param.LookInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Look.canceled += ctx => _param.LookInput = Vector2.zero;
            
            _input.GamePlay.Move.performed += ctx => _param.MoveInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Move.canceled += ctx => _param.MoveInput = Vector2.zero;

            _input.GamePlay.Space.performed += OnSpacePressed;
            _input.GamePlay.Space.canceled += OnSpaceReleased;
        
            //TODO: 加入检测可锁定目标时，再让isLocked的bool改变，以及可锁定目标消失时，isLocked变为false
            _param.IsLocked = false;
            _input.GamePlay.Lock.performed += _ => _param.IsLocked = !_param.IsLocked;
        
            //TODO：这里把翻滚状态设为true之后，需要在对应的单独clip中播放完动画后把isBoil转换为false 
        
        }

        public void Enable()
        {
            _input.Enable();
        }

        public void Stop()
        {
            _input.Disable();
        }

        public void Update()
        {
            _inputBuffer.Update(_param.MoveInput, _param);
            
            if (_spaceHeld && _timerManager.IsFinished(StringConstants.TimerName.SpaceHold))
            {
                _param.Run = true;
            }
        }
       


        private void OnSpacePressed(InputAction.CallbackContext ctx)
        {
            _spaceHeld = true;
            _timerManager.Start(StringConstants.TimerName.SpaceHold, 0.5f);
        }

        private void OnSpaceReleased(InputAction.CallbackContext ctx)
        {
            bool longPress = _timerManager.IsFinished(StringConstants.TimerName.SpaceHold);
            _timerManager.CleanupFinished();
            _spaceHeld = false;

            if (longPress)
            {
                _param.Run = false;
            }
            else
            {
                if (_param.MoveInput.sqrMagnitude <= 0.01f)
                    _param.JumpBackwardTrigger.Set();
                else
                    _param.BoilTrigger.Set();
            }
        }
        
        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            _param.AttackTrigger.Set();
        }
    }
}

