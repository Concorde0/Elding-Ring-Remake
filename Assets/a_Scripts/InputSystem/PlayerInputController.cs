using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;

public class PlayerInputController
{
    private PlayerInput _input;

    private PlayerParam _param;
    public PlayerInputController(PlayerParam param)
    {
        _param = param;
        
        _input = new PlayerInput();
        _input.GamePlay.Move.performed += ctx => _param.moveInput = ctx.ReadValue<Vector2>();
        _input.GamePlay.Move.canceled += ctx => _param.moveInput = Vector2.zero;
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
