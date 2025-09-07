using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_MoveToStartPosition : Action
{
    public SharedVector3 startPosition;
    public SharedFloat stoppingDistance = 0.1f;
    public float turnSpeed = 720f;

    private Animator animator;
    private bool     isMoving;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
        // 记录出生点
        startPosition.Value = transform.position;
    }

    public override void OnStart()
    {
        isMoving = true;
        animator.SetBool("Walk", true);
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 toStart = startPosition.Value - transform.position;
        toStart.y = 0;
        float dist = toStart.magnitude;

        if (dist <= stoppingDistance.Value)
        {
            StopMoving();
            return TaskStatus.Success;
        }

        // 面朝出生点
        Vector3 dir = toStart.normalized;
        Quaternion want = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, want, turnSpeed * Time.deltaTime);

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        StopMoving();
    }

    private void StopMoving()
    {
        if (!isMoving) return;
        animator.SetBool("Walk", false);
        isMoving = false;
    }
    
}