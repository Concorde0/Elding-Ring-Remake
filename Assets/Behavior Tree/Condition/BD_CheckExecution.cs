using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_CheckExecution : Conditional
{
    private CharacterStats stats;

    public override void OnAwake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override TaskStatus OnUpdate()
    {
        if (stats == null) return TaskStatus.Failure;
        return stats.isCritical ? TaskStatus.Success : TaskStatus.Failure;
    }
}