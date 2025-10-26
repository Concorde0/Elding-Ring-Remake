using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_TargetTooFar : Conditional
{
    public SharedTransform target;
    public SharedFloat disengageDistance = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;
        var dist = Vector3.Distance(transform.position, target.Value.position);
        return dist > disengageDistance.Value
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}