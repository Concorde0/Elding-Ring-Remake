using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_CheckStagger : Conditional
{
    private CharacterStats enemyStats;

    public override void OnAwake()
    {
        enemyStats = GetComponent<CharacterStats>();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyStats == null) return TaskStatus.Failure;

        return enemyStats.CheckStagger() ? TaskStatus.Success : TaskStatus.Failure;
    }
}