using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_WithinRange : Conditional {
    public SharedTransform target;
    public SharedFloat range = 1.8f;

    public override TaskStatus OnUpdate() {
        if (target.Value == null) return TaskStatus.Failure;
        return Vector3.Distance(transform.position, target.Value.position) <= range.Value
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}