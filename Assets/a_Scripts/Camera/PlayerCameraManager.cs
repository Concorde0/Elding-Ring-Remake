using System;
using System.Collections.Generic;
using RPG.CameraSystem;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(TargetLockController))]
public class PlayerCameraManager : MonoBehaviour
{
    public CameraResources cameraResources;
    public Transform cameraFollowPoint;
    public int lockCamPriority = 100;
    public int freeLookPriority = 10;

    private FreeLookCameraController freeLookState;
    private LockOnCameraState lockState;
    private ICameraState currentState;

    private TargetLockController targetLockController;
    
    public event Action<Transform> OnLockTargetChanged;

    private void Awake()
    {
        freeLookState = new FreeLookCameraController(cameraResources);
        lockState = new LockOnCameraState(cameraResources.lockVirtualCamera, cameraResources.cinemachineFreeLook, cameraFollowPoint, lockCamPriority, freeLookPriority);

        // 设为初始状态 FreeLook
        currentState = freeLookState;
        currentState.Enter();

        targetLockController = GetComponent<TargetLockController>();
    }

    private void Update()
    {
        currentState?.Tick();

        // 在锁定状态下保持朝向目标，即使玩家没输入
        var target = GetCurrentTarget();
        if (target != null)
        {
            GameLoop.Instance?._player?.Motor?.HandleLockRotation();
        }
    }

    public void ToggleLock()
    {
        if (lockState.GetCurrentTarget() == null)
        {
            //找目标并锁定第一个
            List<Transform> candidates = targetLockController.GetCandidateTargets();
            if (candidates.Count > 0)
            {
                LockTo(candidates[0]);
            }
            else
            {
                Debug.Log("没有可锁定的目标");
            }
        }
        else
        {
            Unlock();
        }
    }

    public void LockTo(Transform t)
    {
        if (t == null)
        {
            return;
        }
        
        lockState.LockTo(t);
        currentState = lockState;
        
        OnLockTargetChanged?.Invoke(t);
        
        FaceTargetImmediately(t);
    }

    public void Unlock()
    {
        lockState.Unlock();
        currentState = freeLookState;

        OnLockTargetChanged?.Invoke(null);
    }
    
    private void FaceTargetImmediately(Transform target)
    {
        var player = GameLoop.Instance?.playerModel;
        if (player == null || target == null)
        {
            return;
        }

        Vector3 dir = target.position - player.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            player.rotation = Quaternion.LookRotation(dir);
        }
    }
    
    public void ForceLockTo(Transform t) => LockTo(t);
    public Transform GetCurrentTarget() => lockState.GetCurrentTarget();
}
