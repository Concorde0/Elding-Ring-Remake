using System;
using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using RPG.MotionSystem;
using UnityEngine;
public class GameLoop : MonoBehaviour
{
    public Transform playerModel;
    public AnimSetting animSetting;
    
    
    public static GameLoop Instance;
    
    private PlayerMotion _player;
    private Animator _animator;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _animator = playerModel.GetComponent<Animator>();
      
    }

    private void Start()
    {
        _player = new PlayerMotion(playerModel,animSetting);   
        _player.Start();
    }

    private void Update()
    {
        _player.Update();
        Debug.Log("Animator.deltaPosition: " + _animator.deltaPosition);
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
