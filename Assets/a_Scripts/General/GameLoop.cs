using System;
using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using RPG.InputSystem;
using RPG.MotionSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLoop : MonoBehaviour
{
    public Transform playerModel;
    public AnimSetting animSetting;
    public CameraResources cameraResources;
    public PlayerCameraManager cameraManager;
    public static GameLoop Instance;
    
    public PlayerMotion _player;

    public GameObject Hitbox;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _player = new PlayerMotion(playerModel, animSetting, cameraResources);
        _player.Start();
        
        cameraManager = FindObjectOfType<PlayerCameraManager>();
        cameraManager.OnLockTargetChanged += _player.OnLockTargetChanged;
        
    }

    private void Update()
    {
        _player.Update();
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
    }
}
