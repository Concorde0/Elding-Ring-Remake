using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    public Transform player;  // 要跟踪的玩家 Transform
    [Tooltip("插值速度（越大越快但抖动可能被带动），典型值 5～20")]
    public float positionLerpSpeed = 10f;
    [Tooltip("如果你也想平滑旋转 / 朝向，可以加旋转插值")]
    public float rotationLerpSpeed = 5f;
    
    private Vector3 _vel = Vector3.zero;

    private void LateUpdate()
    {
        if (player == null)
            return;
        
        Vector3 targetPos = player.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, 1f / positionLerpSpeed);
        
    }
}
