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
        private PlayerParam _param { get; set; }
        private TimerManager _timerManager {get; set;}

        private bool _isPerformedSpace;
        public PlayerInputController(PlayerParam param, TimerManager timerManager)
        {
            _param = param;
            _timerManager = timerManager;
        
            _input = new PlayerInput();
            
            _input.GamePlay.Look.performed += ctx => _param.LookInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Look.canceled += ctx => _param.LookInput = Vector2.zero;
            
            _input.GamePlay.Move.performed += ctx => _param.MoveInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Move.canceled += ctx => _param.MoveInput = Vector2.zero;

            _input.GamePlay.Space.performed += SpaceJudgement;
            _input.GamePlay.Space.canceled += SpaceClear;
        
            //TODO: 加入检测可锁定目标时，再让isLocked的bool改变，以及可锁定目标消失时，isLocked变为false
            _param.IsLocked = false;
            _input.GamePlay.Lock.performed += _ => _param.IsLocked = !_param.IsLocked;
        
            //TODO：这里把翻滚状态设为true之后，需要在对应的单独clip中播放完动画后把isBoil转换为false 
        
        }


        private void SpaceJudgement(InputAction.CallbackContext obj)
        {
            _timerManager.Start(StringConstants.TimerName.SpacePerform, 0.5f);
            _isPerformedSpace = true;
        }
        //TODO: 这里的param的bool值要在动画播放完之后变为false
        private void SpaceClear(InputAction.CallbackContext obj)
        {
            _isPerformedSpace = false;
            _param.Run = false;
            if (!_timerManager.IsFinished(StringConstants.TimerName.SpacePerform))
            {
                if (_param.MoveInput == Vector2.zero)
                {
                    _param.JumpBackward = true;
                }
                else
                {
                    _param.Boil = true;
                }
            }
            
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
            SpaceConditions();
        }

        private void SpaceConditions()
        {
            if (_isPerformedSpace)
            {
                if (_timerManager.IsFinished(StringConstants.TimerName.SpacePerform))
                {
                    _param.Run = true;
                }
            }
        }

        private void GetMoveInput()
        {
        
        }
    }
}

