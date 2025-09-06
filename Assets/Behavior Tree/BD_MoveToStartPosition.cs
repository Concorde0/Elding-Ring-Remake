using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BD_MoveToStartPosition : Action
{
    // 出生点由 OnAwake 记录，无需在 Inspector 预设
    public SharedVector3 startPosition;
    public SharedFloat stoppingDistance = 0.1f;
    public SharedFloat turnSpeed = 10f;

    private NavMeshAgent agent;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        // 记录出生点
        startPosition.Value = transform.position;
        // 允许NavMeshAgent驱动位置，但关闭自动旋转
        agent.updatePosition = true;
        agent.updateRotation = false;
    }

    public override void OnStart()
    {
        agent.isStopped = false;
        agent.stoppingDistance = Mathf.Max(0.01f, stoppingDistance.Value);
        agent.SetDestination(startPosition.Value);
    }

    public override TaskStatus OnUpdate()
    {
        // 如果路径还在计算中，持续Running
        if (agent.pathPending) {
            return TaskStatus.Running;
        }

        //用agent.desiredVelocity计算面向方向
        Vector3 desiredDir = agent.desiredVelocity;
        desiredDir.y = 0f;
        if (desiredDir.sqrMagnitude > 0.0001f) {
            Quaternion targetRot = Quaternion.LookRotation(desiredDir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                turnSpeed.Value * Time.deltaTime);
        }

        //判断是否到达
        return (agent.remainingDistance <= agent.stoppingDistance)
            ? TaskStatus.Success
            : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        agent.isStopped = true;
        //恢复自动旋转
        agent.updateRotation = true;
    }
}