using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    public Transform player;
    [Tooltip("插值速度")]
    public float positionLerpSpeed = 15f;
    [Tooltip("旋转插值")]
    public float rotationLerpSpeed = 5f;
    
    private Vector3 _vel = Vector3.zero;

    private void LateUpdate()
    {
        if (player == null)
            return;
        
        Vector3 targetPos = player.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, 1f / positionLerpSpeed);
        transform.rotation = Quaternion.identity; 
        
        
    }
}