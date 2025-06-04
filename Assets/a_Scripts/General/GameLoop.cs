using System;
using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using RPG.MotionSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLoop : MonoBehaviour
{
    public Transform playerModel;
    public AnimSetting animSetting;
    public CameraResources cameraResources;
    public static GameLoop Instance;
    
    private PlayerMotion _player;
    
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
    }

    private void Update()
    {
        _player.Update();
    }

    private void FixedUpdate()
    {
        _player.FixedUpdate();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        _player.Stop();
    }
}
