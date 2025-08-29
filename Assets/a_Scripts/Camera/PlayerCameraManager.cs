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

    private void Awake()
    {
        if (cameraResources == null) Debug.LogError("CameraResources 未赋值！");
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

    public void CycleTarget(int direction = 1)
    {
        var cand = targetLockController.GetCandidateTargets();
        if (cand.Count == 0) return;
        var current = lockState.GetCurrentTarget();
        if (current == null)
        {
            LockTo(cand[0]);
            return;
        }
        int idx = cand.IndexOf(current);
        if (idx < 0) { LockTo(cand[0]); return; }
        int next = (idx + direction + cand.Count) % cand.Count;
        LockTo(cand[next]);
    }

    public void LockTo(Transform t)
    {
        if (t == null) return;
        
        lockState.LockTo(t);
        currentState = lockState;
        
        SendMessage("OnLockTargetChanged", t, SendMessageOptions.DontRequireReceiver);
    }

    public void Unlock()
    {
        lockState.Unlock();
        currentState = freeLookState;

        SendMessage("OnLockTargetChanged", null, SendMessageOptions.DontRequireReceiver);
    }
    
    public void ForceLockTo(Transform t) => LockTo(t);
    public Transform GetCurrentTarget() => lockState.GetCurrentTarget();
}
