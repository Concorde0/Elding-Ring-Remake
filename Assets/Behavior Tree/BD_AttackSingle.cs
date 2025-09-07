using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class BD_AttackSingle : Action
{
    public SharedTransform target;
    public string triggerName;
    public float fullDuration;
    public float turnSpeed = 500f;
    
    public float minDistance = -1f;
    public float maxDistance = 999f;
    
    // requireInRangeToStart: 是否要求在范围内才能进入这个 Action（用于 Sequence 的早期失败）
    // requireInRangeToTrigger: 是否要求在范围内才能真正触发动画（从 Facing -> Triggered）
    public bool requireInRangeToStart = true;
    public bool requireInRangeToTrigger = true;
    
    public float interruptTime = 0f;      // <=0 表示不启用打断
    public float nextMinDistance = -1f;
    public float nextMaxDistance = 999f;

    private enum State { Facing, Triggered, Cooling }
    private State state;
    private float timer;
    private Animator animator;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        state = State.Facing;
        timer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null || target.Value == null) return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, target.Value.position);
        
        if (requireInRangeToStart)
        {
            bool lowerOk = (minDistance < 0f) ? true : (dist >= minDistance);
            bool upperOk = (maxDistance < 0f) ? true : (dist <= maxDistance);
            if (!(lowerOk && upperOk)) return TaskStatus.Failure;
        }
        
        // 既朝向到位又在本段范围内才会进入 Triggered
        if (state == State.Facing)
        {
            Vector3 dir = target.Value.position - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion want = Quaternion.LookRotation(dir.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, want, turnSpeed * Time.deltaTime);

                bool facingOk = Quaternion.Angle(transform.rotation, want) < 5f;
                if (requireInRangeToTrigger)
                {
                    bool lowerOk = (minDistance < 0f) ? true : (dist >= minDistance);
                    bool upperOk = (maxDistance < 0f) ? true : (dist <= maxDistance);
                    if (facingOk && lowerOk && upperOk)
                    {
                        state = State.Triggered;
                    }
                }
                else
                {
                    if (facingOk)
                    {
                        state = State.Triggered;
                    }
                }
            }
            else
            {
                // 目标位置与自身重合
                if (requireInRangeToTrigger)
                {
                    bool lowerOk = (minDistance < 0f) ? true : (dist >= minDistance);
                    bool upperOk = (maxDistance < 0f) ? true : (dist <= maxDistance);
                    if (lowerOk && upperOk) state = State.Triggered;
                }
                else
                {
                    state = State.Triggered;
                }
            }
            return TaskStatus.Running;
        }
        
        if (state == State.Triggered)
        {
            if (animator == null) return TaskStatus.Failure;
            if (!string.IsNullOrEmpty(triggerName))
            {
                animator.SetTrigger(triggerName);
            }
            timer = 0f;
            state = State.Cooling;
            return TaskStatus.Running;
        }
        
        if (state == State.Cooling)
        {
            timer += Time.deltaTime;
            
            if (interruptTime > 0f && timer >= interruptTime)
            {
                float nextDist = Vector3.Distance(transform.position, target.Value.position);
                bool lowerOk = (nextMinDistance < 0f) ? true : (nextDist >= nextMinDistance);
                bool upperOk = (nextMaxDistance < 0f) ? true : (nextDist <= nextMaxDistance);
                if (lowerOk && upperOk)
                {
                    return TaskStatus.Success;
                }
            }

            if (timer >= fullDuration)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }

    public override void OnEnd()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
        }

        state = State.Facing;
        timer = 0f;
    }
}
