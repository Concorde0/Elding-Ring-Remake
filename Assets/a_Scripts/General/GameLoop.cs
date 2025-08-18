using System;
using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using RPG.InputSystem;
using RPG.MotionSystem;
using RPG.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLoop : MonoBehaviour
{
    public Transform playerModel;
    public AnimSetting animSetting;
    public CameraResources cameraResources;
    public static GameLoop Instance;
    
    private PlayerMotion _player;
    private UIManager _uiManager;
    private UIInputController _uiInputController;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _uiManager = new UIManager();
        _uiManager.Show<MainUIView, MainUIViewModel>(StringConstants.WindowId.MainWindow);
        
        _player = new PlayerMotion(playerModel, animSetting, cameraResources,_uiManager);
        _player.Start();
        
        _uiInputController = new UIInputController(_player.Input.Input, _uiManager);
        _uiInputController.Enable();
    }

    private void Update()
    {
        _player.Update();
        _uiManager.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _player.FixedUpdate();
    }

    private void LateUpdate()
    {
        _player.LateUpdate();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        _player.Stop();
        _uiInputController.Disable();
    }
}
