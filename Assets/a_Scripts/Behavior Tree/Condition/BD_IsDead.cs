using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_IsDead : Conditional
{
    private CharacterStats stats;

    public override void OnAwake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override TaskStatus OnUpdate()
    {
        if (stats == null) return TaskStatus.Failure;
        
        if (stats.CurrentHealth <= 0)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}