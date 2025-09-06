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
    public float turnSpeed = 720f;
    public List<ComboStep> steps = new List<ComboStep>()
    {
        new ComboStep { triggerName="Attack1", fullDuration=3.96f, interruptTime=1.23f, minDistance=0f,   maxDistance=2.0f },
        new ComboStep { triggerName="Attack2", fullDuration=3.13f, interruptTime=1.96f, minDistance=0f,   maxDistance=1.5f },
        new ComboStep { triggerName="Attack3", fullDuration=3.13f, interruptTime=0f,   minDistance=0.5f, maxDistance=1.2f },
    };

    private enum State { Facing, Triggering, Cooling }
    private State  state;
    private int    stepIndex;
    private float  timer;
    private Animator animator;

    public override void OnStart()
    {
        animator  = GetComponent<Animator>();
        stepIndex = 0;
        state     = State.Facing;
        timer     = 0f;

        // 开启 RootMotion，让攻击动画驱动位移
        animator.applyRootMotion = true;
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
            EndCombo();
            return TaskStatus.Success;
        }

        //面朝目标
        if (state == State.Facing)
        {
            Vector3 dir = target.Value.position - transform.position;
            dir.y = 0;
            Quaternion want = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, want, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, want) < 5f)
            {
                // 进入触发阶段
                state = State.Triggering;
            }
            return TaskStatus.Running;
        }

        // 触发本段攻击
        if (state == State.Triggering)
        {
            animator.SetTrigger(step.triggerName);
            timer = 0f;
            state = State.Cooling;
            return TaskStatus.Running;
        }

        //Cooling：等待中断点或满时长
        if (state == State.Cooling)
        {
            timer += Time.deltaTime;
            bool isLast = (stepIndex == steps.Count - 1);

            // 中断点：尝试切下一招
            if (!isLast && step.interruptTime > 0f && timer >= step.interruptTime)
            {
                var next = steps[stepIndex + 1];
                float nextDist = Vector3.Distance(transform.position, target.Value.position);
                if (nextDist >= next.minDistance && nextDist <= next.maxDistance)
                {
                    stepIndex++;
                    state = State.Facing;
                    return TaskStatus.Running;
                }
            }

            // 满时长：推进下一步或结束
            if (timer >= step.fullDuration)
            {
                if (!isLast)
                {
                    stepIndex++;
                    state = State.Facing;
                    return TaskStatus.Running;
                }
                EndCombo();
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        EndCombo();
    }

    // 应用 RootMotion 位移
    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            transform.position += animator.deltaPosition;
        }
    }

    // 清理状态、Reset Trigger
    private void EndCombo()
    {
        animator.applyRootMotion = false;
        foreach (var s in steps)
        {
            animator.ResetTrigger(s.triggerName);
        }
    }
}
