using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_MoveToTarget : Action
{
    public SharedTransform target;
    public SharedFloat stoppingDistance = 1.8f;
    public float turnSpeed = 720f;

    private Animator animator;
    private bool     isMoving;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        isMoving = true;
        animator.SetBool("Walk", true);
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;

        Vector3 toTarget = target.Value.position - transform.position;
        toTarget.y = 0;
        float dist = toTarget.magnitude;

        // 到达停止距离
        if (dist <= stoppingDistance.Value)
        {
            StopMoving();
            return TaskStatus.Success;
        }

        // 面朝目标
        Vector3 dir = toTarget.normalized;
        Quaternion want = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, want, turnSpeed * Time.deltaTime);

        // RootMotion 本身负责向前位移
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        StopMoving();
    }

    private void StopMoving()
    {
        if (!isMoving) return;
        animator.SetBool("Walk" , false);
        animator.applyRootMotion = false;
        isMoving = false;
    }

    // 把动画根运动量应用到 Transform
    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            transform.position += animator.deltaPosition;
        }
    }
}