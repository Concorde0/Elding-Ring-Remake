using System;
using System.Collections;
using System.Collections.Generic;
using RPG.InputSystem;
using UnityEngine;
using RPG.InputSystem;
public class GameLoop : MonoBehaviour
{
    public InputData inputData;
    
    private InputManager _inputManager;

    private void Awake()
    {
        _inputManager = new InputManager(inputData);
    }

    private void Update()
    {
        _inputManager.Update(); 
    }
}
