using System;
using System.Collections;
using System.Collections.Generic;
using RPG.MotionSystem;
using UnityEngine;
public class GameLoop : MonoBehaviour
{
    public Transform playerModel;
    public AnimationClip[] idleClips;
    
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
        _player = new PlayerMotion(playerModel, idleClips);
    }

    private void Update()
    {
        _player.Update();
    }

    private void OnDisable()
    {
        _player.Stop();
    }
}
