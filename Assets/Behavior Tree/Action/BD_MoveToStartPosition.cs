using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_MoveToStartPosition : Action
{
    public SharedVector3 startPosition;
    public SharedFloat stoppingDistance = 0.1f;
    public float turnSpeed = 720f;

    // 新增：检测玩家（目标）和攻击范围
    public SharedTransform target;
    public SharedFloat attackRange;

    private Animator animator;
    private bool     isMoving;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
        
        if (startPosition == null)
        {
            startPosition = new SharedVector3();
        }
        startPosition.Value = transform.position;
    }

    public override void OnStart()
    {
        if (animator == null) animator = GetComponent<Animator>();

        isMoving = true;
        if (animator != null) animator.SetBool("Walk", true);
    }

    public override TaskStatus OnUpdate()
    {
        if (target != null && target.Value != null)
        {
            float distToTarget = Vector3.Distance(transform.position, target.Value.position);
            if (distToTarget <= attackRange.Value)
            {
                StopMoving();
                return TaskStatus.Failure;
            }
        }
        
        if (startPosition == null)
        {
            StopMoving();
            return TaskStatus.Failure;
        }

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
        if (animator != null) animator.SetBool("Walk", false);
        isMoving = false;
    }
}
