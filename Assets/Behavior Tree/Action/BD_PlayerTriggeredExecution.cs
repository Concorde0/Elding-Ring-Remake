using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_PlayerTriggeredExecution : Conditional
{
    public SharedTransform player;
    public float executionRange = 2f;

    private CharacterStats stats;

    public override void OnAwake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override TaskStatus OnUpdate()
    {
        if (stats == null || !stats.isCritical) return TaskStatus.Failure;
        if (player.Value == null) return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, player.Value.position);
        if (dist > executionRange) return TaskStatus.Failure;
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            stats.isCritical = false;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}