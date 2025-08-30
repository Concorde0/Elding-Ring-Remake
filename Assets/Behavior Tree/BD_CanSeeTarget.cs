using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_CanSeeTarget : Conditional {
    public SharedTransform target;
    public SharedFloat sightRadius = 15f;
    public SharedFloat fov = 120f;
    public LayerMask obstacleMask = ~0;
    public Transform eye; // 可空

    public override TaskStatus OnUpdate() {
        Debug.Log($"[CanSeeTarget] target={target.Value}, dist={Vector3.Distance(transform.position, target.Value.position)}");
        if (target.Value == null) return TaskStatus.Failure;

        Vector3 origin = eye ? eye.position : transform.position + Vector3.up * 1.6f;
        Vector3 to = target.Value.position - origin;
        float dist = to.magnitude;
        if (dist > sightRadius.Value) return TaskStatus.Failure;

        Vector3 dir = to.normalized;
        if (Vector3.Angle(transform.forward, dir) > fov.Value * 0.5f) return TaskStatus.Failure;

        if (Physics.Raycast(origin, dir, out var hit, dist, obstacleMask)) {
            if (hit.transform != target.Value && !hit.transform.IsChildOf(target.Value)) return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }
    
    public bool OnCheck() {
        return OnUpdate() == TaskStatus.Success;
    }
}