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

        private bool _isHoldSpace;
        public PlayerInputController(PlayerParam param, TimerManager timerManager)
        {
            _param = param;
            _timerManager = timerManager;
            _input = new PlayerInput();

            _input.GamePlay.Attack.performed += OnAttackPerformed;
            
            _input.GamePlay.Move.performed += ctx => _param.MoveInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Move.canceled += ctx => _param.MoveInput = Vector2.zero;

            _input.GamePlay.Space.performed += OnSpacePressed;
            _input.GamePlay.Space.canceled += OnSpaceReleased;
            
            _input.GamePlay.Inventory.performed += ctx => { };
        
            //TODO: 加入检测可锁定目标时，再让isLocked的bool改变，以及可锁定目标消失时，isLocked变为false
            _param.IsLocked = false;
            _input.GamePlay.Lock.performed += _ => _param.IsLocked = !_param.IsLocked;

            _input.GamePlay.Hurt.performed += _ => _param.IsSpecialHurt = true;

            _input.GamePlay.LionSmash.performed += _ => _param.IsLion = true;


            //TODO：这里把翻滚状态设为true之后，需要在对应的单独 clip 中播放完动画后把 isBoil 转换为 false 

        }
        
        
        public PlayerInput Input => _input;
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
            SpaceLogic();
        }
        
       


        private void OnSpacePressed(InputAction.CallbackContext ctx)
        {
            _isHoldSpace = true;
            _timerManager.Start(StringConstants.TimerName.SpaceHold,0.5f);
            _timerManager.Start(StringConstants.TimerName.BoilTimeHold,0.1f);
            
        }

        private void OnSpaceReleased(InputAction.CallbackContext ctx)
        {
            _isHoldSpace = false;
            _param.Run = false;
        }
        
        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            _param.AttackTrigger.Set();
        }

        private void SpaceLogic()
        {
            if (_isHoldSpace && _timerManager.IsFinished(StringConstants.TimerName.SpaceHold))
            {
                _param.Run = true;
            }
            
            
            if (_isHoldSpace 
                && !_param.IsInBoil
                && _param.MoveInput == Vector2.zero 
                && _timerManager.IsFinished(StringConstants.TimerName.BoilTimeHold) 
                && !_timerManager.IsFinished(StringConstants.TimerName.SpaceHold))
            {
                _param.JumpBackwardTrigger.Set();
                _timerManager.CleanupFinished();
            }

            if (!_isHoldSpace
                && !_param.IsInBoil
                && _param.MoveInput == Vector2.zero 
                && _timerManager.Exists(StringConstants.TimerName.SpaceHold) 
                && !_timerManager.IsFinished(StringConstants.TimerName.SpaceHold))
            {
                _param.JumpBackwardTrigger.Set();
                _timerManager.CleanupFinished();
            }
            else if (!_isHoldSpace
                     && !_param.IsInBoil
                     && _param.MoveInput.sqrMagnitude > 0.1f
                     && _timerManager.Exists(StringConstants.TimerName.SpaceHold)
                     && !_timerManager.IsFinished(StringConstants.TimerName.SpaceHold))
            {
                _param.BoilTrigger.Set();
                _timerManager.CleanupFinished();
            }
        }
    }
}

