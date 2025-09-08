using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_WaitForRange : Action
{
    public SharedTransform target;
    public float minRange = 6f;
    public float maxRange = 10f;
    public float timeout = 1f;

    private float timer;

    public override void OnStart()
    {
        timer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null || target.Value == null) return TaskStatus.Failure;

        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, target.Value.position);

        if (dist >= minRange && dist <= maxRange)
        {
            return TaskStatus.Success;
        }

        if (timer >= timeout)
        {
            return TaskStatus.Failure;
        }
        
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        timer = 0f;
    }
}