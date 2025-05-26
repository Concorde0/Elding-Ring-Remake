using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;


namespace RPG.InputSystem
{
    public class PlayerInputController
    {
        private readonly PlayerInput _input;

        public PlayerParam _param { get; private set; }
        public PlayerInputController(PlayerParam param)
        {
            _param = param;
        
            _input = new PlayerInput();
            
            _input.GamePlay.Look.performed += ctx => _param.lookInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Look.canceled += ctx => _param.lookInput = Vector2.zero;
            
            _input.GamePlay.Move.performed += ctx => _param.moveInput = ctx.ReadValue<Vector2>();
            _input.GamePlay.Move.canceled += ctx => _param.moveInput = Vector2.zero;
        
            _input.GamePlay.Run.performed  += _ => _param.run = true;
            _input.GamePlay.Run.canceled   += _ => _param.run = false;
        
            //TODO: 加入检测可锁定目标时，再让isLocked的bool改变，以及可锁定目标消失时，isLocked变为false
            _param.isLocked = false;
            _input.GamePlay.Lock.performed += _ => _param.isLocked = !_param.isLocked;
        
            //TODO：这里把翻滚状态设为true之后，需要在对应的单独clip中播放完动画后把isBoil转换为false 
            _input.GamePlay.Boil.performed += _ => _param.isBoil = true;
        
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
        
        }

        private void GetMoveInput()
        {
        
        }
    }
}

