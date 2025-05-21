using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerInputController
{
    private readonly PlayerInput _input;

    private PlayerParam _param;
    public PlayerInputController(PlayerParam param)
    {
        _param = param;
        
        _input = new PlayerInput();
        _input.GamePlay.Move.performed += ctx => _param.moveInput = ctx.ReadValue<Vector2>();
        _input.GamePlay.Move.canceled += ctx => _param.moveInput = Vector2.zero;
        
        _input.GamePlay.Run.performed  += _ => _param.run = true;
        _input.GamePlay.Run.canceled   += _ => _param.run = false;
        
        //TODO: 加入检测可锁定目标时，再让isLocked的bool改变，以及可锁定目标消失时，isLocked变为false
        _param.isLocked = false;
        _input.GamePlay.Lock.performed += _ => _param.isLocked = !_param.isLocked;
        
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
