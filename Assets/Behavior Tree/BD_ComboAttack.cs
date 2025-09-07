using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

[Serializable]
public struct ComboStep
{
    public string triggerName;
    public float fullDuration;
    public float interruptTime;
    public float minDistance;
    public float maxDistance;
}

public class BD_ComboAttack : Action
{
    public SharedTransform target;
    public float turnSpeed = 500f;
    public List<ComboStep> steps = new List<ComboStep>()
    {
        new ComboStep { triggerName="Attack1", fullDuration=3.96f, interruptTime=1.1f, minDistance=-1f,   maxDistance=2.0f },
        new ComboStep { triggerName="Attack2", fullDuration=3.13f, interruptTime=1.96f, minDistance=-1f,   maxDistance=1.5f },
        new ComboStep { triggerName="Attack3", fullDuration=3.13f, interruptTime=0f,   minDistance=0.5f, maxDistance=1.2f },
    };

    private enum State { Facing, Triggering, Cooling }
    private State  state;
    private int    stepIndex;
    private float  timer;
    private Animator animator;
    private bool isTriggered;
    private bool hasAdvanced;

    public override void OnStart()
    {
        animator  = GetComponent<Animator>();
        stepIndex = 0;
        state     = State.Facing;
        timer     = 0f;
        hasAdvanced = false;
        isTriggered = false;
        
    }

    public override TaskStatus OnUpdate()
    {
        
        if (target.Value == null)
            return TaskStatus.Failure;

        var step = steps[stepIndex];
        float dist = Vector3.Distance(transform.position, target.Value.position);

        
        // 超出本段距离区间，退出 Combo
        if (dist < step.minDistance || dist > step.maxDistance)
        {
            return TaskStatus.Success;
        }

        //面朝目标
        if (state == State.Facing)
        {
            Vector3 dir = target.Value.position - transform.position;
            dir.y = 0;
            Quaternion want = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, want, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, want) < 5f)
            {
                // 进入触发阶段
                state = State.Triggering;
            }
            return TaskStatus.Running;
        }

        // 触发本段攻击
        if (state == State.Triggering && !isTriggered)
        {
            animator.SetTrigger(step.triggerName);
            timer = 0f;
            state = State.Cooling;
            hasAdvanced = false; 
            isTriggered = true;
            return TaskStatus.Running;
        }

        //Cooling：等待中断点或满时长
        if (state == State.Cooling)
        {
            timer += Time.deltaTime;
            bool isLast = (stepIndex == steps.Count - 1);

            if (!hasAdvanced)
            {
                // 中断点：尝试切下一招
                if (!isLast && step.interruptTime > 0f && timer >= step.interruptTime)
                {
                    var next = steps[stepIndex + 1];
                    float nextDist = Vector3.Distance(transform.position, target.Value.position);
                    if (nextDist >= next.minDistance && nextDist <= next.maxDistance)
                    {
                        Debug.Log("Try Advance");
                        stepIndex++;
                        state = State.Facing;
                        hasAdvanced = true;
                        isTriggered = false;
                        return TaskStatus.Running;
                    }
                }

                // 满时长：推进下一步或结束
                if (timer >= step.fullDuration)
                {
                    if (!isLast)
                    {
                        Debug.Log("fullDuration Advance");
                        stepIndex++;
                        state = State.Facing;
                        hasAdvanced = true;
                        isTriggered = false;
                        return TaskStatus.Running;
                    }
                    return TaskStatus.Running;
                }
            }
            return TaskStatus.Running;
        }
        return TaskStatus.Running;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnEnd()
    {
        hasAdvanced = false;
        isTriggered = false;
        timer = 0f;
    }
    
  
   
}
