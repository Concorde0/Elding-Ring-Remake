using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_SpecialAttackCondition : Conditional
{
    
    public SharedTransform target;
    public float minDistance = 0f;
    public float maxDistance = 999f;

    [Range(0f, 100f)]
    public float triggerChance = 30f;
    
    public float requiredFacingAngle = 180f; 

    public override TaskStatus OnUpdate()
    {
        if (target == null || target.Value == null) return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, target.Value.position);
        if (dist < minDistance || dist > maxDistance) return TaskStatus.Failure;

        if (requiredFacingAngle < 180f)
        {
            Vector3 toTarget = (target.Value.position - transform.position);
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude < 0.0001f) return TaskStatus.Failure;
            float angle = Vector3.Angle(transform.forward, toTarget.normalized);
            if (angle > requiredFacingAngle * 0.5f) return TaskStatus.Failure;
        }

        bool chancePassed = UnityEngine.Random.value * 100f <= triggerChance;
        return chancePassed ? TaskStatus.Success : TaskStatus.Failure;
    }
}