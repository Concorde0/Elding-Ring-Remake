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
    public float turnSpeed = 750f;

    public float minDistance = -1f;
    public float maxDistance = 999f;

    public bool requireInRangeToStart = true;
    public bool requireInRangeToTrigger = true;

    public float interruptTime = 0f;
    public float nextMinDistance = -1f;
    public float nextMaxDistance = 999f;

    public float faceStartTime1 = 0f;
    public float faceEndTime1 = 0f;
    public float faceStartTime2 = 0f;
    public float faceEndTime2 = 0f;

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
            bool lowerOk = (minDistance < 0f) || (dist >= minDistance);
            bool upperOk = (maxDistance < 0f) || (dist <= maxDistance);
            if (!(lowerOk && upperOk)) return TaskStatus.Failure;
        }

        if (state == State.Facing)
        {
            if (requireInRangeToTrigger)
            {
                bool lowerOk = (minDistance < 0f) || (dist >= minDistance);
                bool upperOk = (maxDistance < 0f) || (dist <= maxDistance);
                if (lowerOk && upperOk)
                {
                    state = State.Triggered;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }
            else
            {
                state = State.Triggered;
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

            if ((timer >= faceStartTime1 && timer <= faceEndTime1) ||
                (timer >= faceStartTime2 && timer <= faceEndTime2))
            {
                Vector3 dir = target.Value.position - transform.position;
                dir.y = 0;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    Quaternion want = Quaternion.LookRotation(dir.normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, want, turnSpeed * Time.deltaTime);
                }
            }

            if (interruptTime > 0f && timer >= interruptTime)
            {
                float nextDist = Vector3.Distance(transform.position, target.Value.position);
                bool lowerOk = (nextMinDistance < 0f) || (nextDist >= nextMinDistance);
                bool upperOk = (nextMaxDistance < 0f) || (nextDist <= nextMaxDistance);
                if (lowerOk && upperOk) return TaskStatus.Success;
            }

            if (timer >= fullDuration) return TaskStatus.Success;

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
