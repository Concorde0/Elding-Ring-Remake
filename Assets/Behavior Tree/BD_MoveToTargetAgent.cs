using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BD_MoveToTargetAgent : Action {
    public SharedTransform target;
    public SharedFloat stoppingDistance = 1.8f;

    private NavMeshAgent agent;

    public override void OnAwake() {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnStart() {
        agent.stoppingDistance = Mathf.Max(0.01f, stoppingDistance.Value);
        agent.isStopped = false;
    }

    public override TaskStatus OnUpdate() {
        if (target.Value == null) return TaskStatus.Failure;

        agent.SetDestination(target.Value.position);

        if (!agent.pathPending &&
            agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, agent.radius)) {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd() {
        if (agent) agent.isStopped = true;
    }
}