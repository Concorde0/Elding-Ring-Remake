using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class RootMotionDriver : MonoBehaviour
{
    Animator _anim;
    Rigidbody _rb;
    Vector3   _accumulatedPosition;    // 我们实际用来驱动 Rigidbody 的位置
    private Transform _rootBone;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb   = GetComponent<Rigidbody>();
        _anim.applyRootMotion = true;
        // 初始化到当前刚体位置
        _accumulatedPosition = _rb.position;
        _rootBone = transform.Find("Dummy001");
    }
    void Update()
    {
        
    }

    void OnAnimatorMove()
    {
        // 1) 取出本帧 Animator 算出的根运动增量
        Vector3 delta = _anim.deltaPosition;
        Quaternion rotDelta = _anim.deltaRotation;

        // 2) 根据需求屏蔽垂直轴
        delta.y = 0;

        // 3) 累加到我们的逻辑位置
        _accumulatedPosition += delta;
        Quaternion newRot = _rb.rotation * rotDelta;

        // 4) 用 Rigidbody.MoveXxx 移动，保持物理一致性
        _rb.MovePosition(_accumulatedPosition);
        _rb.MoveRotation(newRot);

        // 5) **把 Animator.transform 的位置“锁回来”**，防止它自己又把根运动叠加上去
        _anim.transform.position = _accumulatedPosition;
        _anim.transform.rotation = newRot;
    }
}
