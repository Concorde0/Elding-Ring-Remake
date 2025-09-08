using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class BD_SpecialAttack : Action
{
    public SharedTransform target;
    public string triggerName;
    public float fullDuration = 1.5f;
    public float turnSpeed = 750f;
    
    public float faceStartTime1 = 0f;
    public float faceEndTime1 = 0.5f;
    public float faceStartTime2 = 0f;
    public float faceEndTime2 = 0f;
    
    public bool uninterruptible = true;
    
    public bool allowEarlySuccess = false;
    public float interruptTime = 0f;
    public float nextMinDistance = -1f;
    public float nextMaxDistance = 999f;

    private float timer;
    private Animator animator;
    private enum State { Started, Playing }
    private State state;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        timer = 0f;
        state = State.Started;
        
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
        state = State.Playing;
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        
        if (target != null && target.Value != null)
        {
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
        }
        
        if (allowEarlySuccess && interruptTime > 0f && timer >= interruptTime)
        {
            if (target == null || target.Value == null)
            {
                if (!uninterruptible)
                {
                    return TaskStatus.Success;
                }
            }
            else
            {
                float nextDist = Vector3.Distance(transform.position, target.Value.position);
                bool lowerOk = (nextMinDistance < 0f) || (nextDist >= nextMinDistance);
                bool upperOk = (nextMaxDistance < 0f) || (nextDist <= nextMaxDistance);
                if (lowerOk && upperOk)
                {
                    return TaskStatus.Success;
                }
            }
        }
        
        if (timer >= fullDuration) return TaskStatus.Success;
        
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
        }
        timer = 0f;
        state = State.Started;
    }
}
